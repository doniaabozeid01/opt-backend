using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.TextRequestsParser.Dtos;

namespace optimum.service.TextRequestsParser
{
    public class TextRequestParserService : ITextRequestParserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public TextRequestParserService(
            IUnitOfWork unitOfWork,
            HttpClient httpClient,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _config = config;
        }




        // ========= ENTRY POINT =========
        public async Task<List<AIParsedRequestItems>> ParseAsync (SchoolRequests request)
        {
            // لو مفيش نص أصلاً، رجّع فاضي
            if (string.IsNullOrWhiteSpace(request.TextContent))
                return new List<AIParsedRequestItems>();

            // نجرب نستخدم DeepSeek لو في API Key
            var hasAiKey = !string.IsNullOrWhiteSpace(_config["DeepSeek:ApiKey"]);

            if (hasAiKey)
            {
                try
                {
                    var products = await _unitOfWork.Repository<Products>().GetAllAsync();
                    var aiItems = await CallDeepSeekParserAsync(request.TextContent, products);

                    if (aiItems != null && aiItems.Any())
                    {
                        return aiItems.Select(i => new AIParsedRequestItems
                        {
                            SchoolRequestId = request.Id,
                            ProductId = i.ProductId,
                            ExtractedName = i.ExtractedName,
                            Quantity = i.Quantity,
                            Notes = i.Notes,
                            Confidence = 1
                        }).ToList();
                    }
                }
                catch (Exception)
                {
                    // لو الـ AI وقع (رصيد، نت، ..) هنرجع للمنطق اليدوي
                }
            }

            // لو مفيش AI Key أو حصل Error → نرجع للمنطق القديم
            return await ParseWithLocalLogicAsync(request);
        }

        // ========= 1) المنطق اليدوي (Regex + Keywords) =========
        //private async Task<List<AIParsedRequestItems>> ParseWithLocalLogicAsync(SchoolRequests request)
        //{
        //    var text = NormalizeArabic(request.TextContent ?? "");
        //    var results = new List<AIParsedRequestItems>();

        //    // 1) Load Products
        //    var products = await _unitOfWork.Repository<Products>().GetAllAsync();

        //    var normalizedProducts = products.Select(p => new
        //    {
        //        Product = p,
        //        Name = NormalizeArabic(p.Name),
        //        Keywords = string.IsNullOrEmpty(p.Keywords)
        //            ? Array.Empty<string>()
        //            : p.Keywords.Split(',').Select(k => NormalizeArabic(k)).ToArray()
        //    }).ToList();

        //    // 2) Split Text على:
        //    // - سطر جديد / نقط / فواصل
        //    // - " و " (واو العطف) ككلمة مستقلة
        //    // - " ثم "
        //    // - " وكمان "
        //    var lines = Regex.Split(
        //                     text,
        //                     @"\sو\s+|\sثم\s+|\sوكمان\s+|[\n\.,،/\-]+")
        //                     .Select(l => l.Trim())
        //                     .Where(l => !string.IsNullOrWhiteSpace(l))
        //                     .ToList();

        //    foreach (var line in lines)
        //    {
        //        int quantity = ExtractNumber(line);

        //        var match = FindProductMatch(line, normalizedProducts);

        //        if (match == null)
        //        {
        //            results.Add(new AIParsedRequestItems
        //            {
        //                SchoolRequestId = request.Id,
        //                ProductId = null,
        //                ExtractedName = line,
        //                Quantity = quantity,
        //                Confidence = 0
        //            });
        //            continue;
        //        }

        //        var productMatch = match.Value;

        //        results.Add(new AIParsedRequestItems
        //        {
        //            SchoolRequestId = request.Id,
        //            ProductId = productMatch.Product.Id,
        //            ExtractedName = productMatch.Product.Name,
        //            Quantity = quantity,
        //            Confidence = productMatch.Confidence
        //        });
        //    }

        //    return results;
        //}




        private async Task<List<AIParsedRequestItems>> ParseWithLocalLogicAsync(SchoolRequests request)
        {
            var text = NormalizeArabic(request.TextContent ?? "");
            var results = new List<AIParsedRequestItems>();

            // 1) Load Products
            var products = await _unitOfWork.Repository<Products>().GetAllAsync();

            if (products == null || !products.Any())
                return results;

            // أول منتج هنستخدمه كـ default لو ملقيناش ماتش
            var firstProduct = products.First();

            // Normalize المنتج مرة واحدة
            var normalizedProducts = products.Select(p => new NormalizedProduct
            {
                Product = p,
                NormalizedName = NormalizeArabic(p.Name),
                Keywords = string.IsNullOrEmpty(p.Keywords)
                    ? Array.Empty<string>()
                    : p.Keywords
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(k => NormalizeArabic(k))
                        .ToArray()
            }).ToList();

            // 2) تقسيم النص لأسطر / أجزاء
            var lines = Regex.Split(
                             text,
                             @"\sو\s+|\sثم\s+|\sوكمان\s+|[\n\.,،/\-]+")
                             .Select(l => l.Trim())
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .ToList();

            foreach (var line in lines)
            {
                int quantity = ExtractNumber(line);

                var match = FindProductMatch(line, normalizedProducts);

                if (match == null)
                {
                    // 🟢 لو فقدنا الأمل في الماتش → استخدم أول منتج
                    results.Add(new AIParsedRequestItems
                    {
                        SchoolRequestId = request.Id,
                        ProductId = firstProduct.Id,
                        ExtractedName = line,           // النص اللي اتكتب
                        Quantity = quantity,
                        Confidence = 0                 // ثقة قليلة
                    });
                    continue;
                }

                var (product, confidence) = match.Value;

                results.Add(new AIParsedRequestItems
                {
                    SchoolRequestId = request.Id,
                    ProductId = product.Id,
                    ExtractedName = product.Name,      // اسم المنتج الرسمي
                    Quantity = quantity,
                    Confidence = confidence
                });
            }

            return results;
        }























        // ========= 2) استدعاء DeepSeek (اختياري) =========
        private async Task<List<AiParsedItemDto>> CallDeepSeekParserAsync(
            string rawText,
            IEnumerable<Products> products)
        {
            // نحضّر لستة المنتجات في نص بسيط
            var productsText = string.Join("\n", products.Select(p =>
                $"- id: {p.Id}, name: {p.Name}, keywords: {p.Keywords}"
            ));

            var prompt = $@"
انت مساعد يقوم بتحويل طلبات مكتوبة بالعربية إلى عناصر طلب منظمة.

عندك قائمة المنتجات التالية من قاعدة البيانات (كل منتج له id):
{productsText}

وهذا نص الطلب من المدرسة (قد يحتوي على أكثر من منتج):
{rawText}

المطلوب:
1. استخرج كل المنتجات المطلوبة من النص.
2. حاول ربط كل عنصر بمنتج من القائمة باستخدام الاسم أو الكلمات المفتاحية.
3. لو لم تجد منتج مناسب اجعل productId = null.
4. quantity يجب أن يكون رقمًا من النص، ولو لم يوجد رقم اجعله 1.
5. extractedName يكون النص الذي يصف المنتج كما فهمته.
6. notes ضع أي تفاصيل إضافية (مثل الصف، المرحلة، أو أي ملاحظات).

ارجع الناتج في صورة JSON فقط (بدون أي نص آخر) ويكون عبارة عن Array من العناصر بهذا الشكل:

[
  {{
    ""productId"": 1,
    ""extractedName"": ""نص اسم المنتج"",
    ""quantity"": 5,
    ""notes"": ""تفاصيل أخرى أو null""
  }}
]
";

            var body = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                    new { role = "system", content = "انت مساعد يساعد في تحويل طلبات مكتوبة بالعربية الى عناصر طلب منظمة." },
                    new { role = "user", content = prompt }
                },
                response_format = new { type = "json_object" }
            };

            var requestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.deepseek.com/v1/chat/completions"
            );

            requestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    _config["DeepSeek:ApiKey"]);

            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"DeepSeek error: {response.StatusCode}, body: {json}");
            }

            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
                return new List<AiParsedItemDto>();

            var items = JsonSerializer.Deserialize<List<AiParsedItemDto>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<AiParsedItemDto>();

            return items;
        }

        // ========= Helpers (Normalize / Regex) =========
        private string NormalizeArabic(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            string text = input;

            string[] fillerWords =
            {
                "عايز", "عايزين", "عاوز", "عاوزين", "محتاج", "محتاجين",
                "نريد", "أريد", "ابي", "ابغى", "لو سمحت", "من فضلك", "رجاء",
                "كمان", "وكمان"
            };

            foreach (var f in fillerWords)
                text = text.Replace(f, " ");

            text = text.Replace("أ", "ا")
                       .Replace("إ", "ا")
                       .Replace("آ", "ا")
                       .Replace("ة", "ه")
                       .Replace("ى", "ي");

            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text.ToLower();
        }

        private int ExtractNumber(string text)
        {
            var m = Regex.Match(text, @"\d+");
            return m.Success ? int.Parse(m.Value) : 1;
        }

        //private (Products Product, double Confidence)? FindProductMatch(
        //    string line,
        //    IEnumerable<dynamic> normalizedProducts
        //)
        //{
        //    double bestScore = 0;
        //    Products bestProduct = null;

        //    var normalizedLine = NormalizeArabic(line);

        //    foreach (var np in normalizedProducts)
        //    {
        //        var product = np.Product;
        //        var keywords = np.Keywords as string[] ?? Array.Empty<string>();

        //        int matchCount = 0;

        //        foreach (var keyword in keywords)
        //        {
        //            if (string.IsNullOrWhiteSpace(keyword)) continue;
        //            if (normalizedLine.Contains(keyword))
        //                matchCount++;
        //        }

        //        double score = (double)matchCount / Math.Max(1, keywords.Length);

        //        if (score > bestScore)
        //        {
        //            bestScore = score;
        //            bestProduct = product;
        //        }
        //    }

        //    if (bestProduct == null || bestScore < 0.2)
        //        return null;

        //    return (bestProduct, bestScore);
        //}



        private (Products Product, double Confidence)? FindProductMatch(
    string line,
    IEnumerable<NormalizedProduct> normalizedProducts
)
        {
            var normalizedLine = NormalizeArabic(line);

            double bestScore = 0;
            Products bestProduct = null;

            foreach (var np in normalizedProducts)
            {
                double score = 0;

                // 1) تطابق الاسم بالكامل أو جزئيًا
                if (!string.IsNullOrWhiteSpace(np.NormalizedName) &&
                    normalizedLine.Contains(np.NormalizedName))
                {
                    score += 2.0; // وزن أعلى للاسم
                }

                // 2) تطابق الـ Keywords
                if (np.Keywords != null && np.Keywords.Length > 0)
                {
                    int matchCount = 0;

                    foreach (var keyword in np.Keywords)
                    {
                        if (string.IsNullOrWhiteSpace(keyword)) continue;
                        if (normalizedLine.Contains(keyword))
                            matchCount++;
                    }

                    if (matchCount > 0)
                    {
                        // نسبة التطابق بالنسبة لعدد الـ keywords
                        score += (double)matchCount / np.Keywords.Length;
                    }
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestProduct = np.Product;
                }
            }

            // لو مفيش أي حاجة مقنعة
            if (bestProduct == null || bestScore < 0.2)
                return null;

            return (bestProduct, bestScore);
        }




        private class NormalizedProduct
        {
            public Products Product { get; set; }
            public string NormalizedName { get; set; }
            public string[] Keywords { get; set; }
        }







        public async Task<(int? productId, double confidence)> PredictProductIdFromNameAsync(string productText)
        {
            var results = await PredictProductsForLinesAsync(new List<string> { productText });

            var first = results.FirstOrDefault();
            if (first.ProductId == null)
                return (null, 0);

            return (first.ProductId, first.Confidence);
        }


        private async Task<List<(int? ProductId, double Confidence)>> PredictProductsForLinesAsync(
        List<string> lines)
        {
            var results = new List<(int? ProductId, double Confidence)>();

            // 1) Load Products
            var products = await _unitOfWork.Repository<Products>().GetAllAsync();

            if (products == null || !products.Any())
                return results;

            var normalizedProducts = products.Select(p => new NormalizedProduct
            {
                Product = p,
                NormalizedName = NormalizeArabic(p.Name),
                Keywords = string.IsNullOrEmpty(p.Keywords)
                    ? Array.Empty<string>()
                    : p.Keywords
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(k => NormalizeArabic(k))
                        .ToArray()
            }).ToList();

            foreach (var line in lines)
            {
                var match = FindProductMatch(line, normalizedProducts);

                if (match == null)
                {
                    // مفيش ماتش مقنع → نخليه null واليوزر يأكد
                    results.Add((null, 0));
                    continue;
                }

                var (product, confidence) = match.Value;
                results.Add((product.Id, confidence));
            }

            return results;
        }


    }
}
