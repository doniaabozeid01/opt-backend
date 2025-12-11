using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.SupplierOfferParser.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace optimum.service.SupplierOfferParser
{
    public class SupplierOfferParserService : ISupplierOfferParserService
    {

        private readonly IUnitOfWork _unitOfWork;

        public SupplierOfferParserService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Task<ParsedSupplierOfferDto> ParseAsync(SupplierOffers offer, SupplierRequest supplierRequest)
        {
            var result = new ParsedSupplierOfferDto();

            if (string.IsNullOrWhiteSpace(offer.RawText) || supplierRequest?.Items == null)
                return Task.FromResult(result);

            var rawText = offer.RawText;
            var text = rawText.ToLower();

            // 1) ملاحظات المورد = النص كله مؤقتًا
            result.SuggestedNotes = rawText;

            // 2) نطلّع مدة التسليم من النص (أي رقم قبل كلمة يوم/أيام)
            result.SuggestedDeliveryDays = ExtractDeliveryDays(text);

            // 3) نطلّع الأرقام اللي هنعتبرها أسعار
            var allNumbers = ExtractAllNumbers(text);

            // لو لقينا deliveryDays بنشيل واحد من الأرقام دي لو هو نفس الرقم
            if (result.SuggestedDeliveryDays.HasValue)
            {
                var idx = allNumbers.FindIndex(n => n == result.SuggestedDeliveryDays.Value);
                if (idx >= 0) allNumbers.RemoveAt(idx);
            }

            // 4) نعتبر اللي باقي أسعار للوحدات
            // هنحطهم بالترتيب، وبعدين نربطهم بـ Items على حسب الـ index
            result.UnitPrices = allNumbers.Select(n => (decimal)n).ToList();

            // 5) نحسب إجمالي تكلفة الشراء SuggestedPurchaseCost
            var items = supplierRequest.Items.ToList();
            decimal total = 0m;

            for (int i = 0; i < items.Count && i < result.UnitPrices.Count; i++)
            {
                var quantity = items[i].Quantity;
                var unitPrice = result.UnitPrices[i];
                total += quantity * unitPrice;
            }

            if (total > 0)
                result.SuggestedPurchaseCost = total;

            // RawJson نسيبه فاضي دلوقتي، لما تدخلي AI تحطي فيه JSON
            result.RawJson = string.Empty;

            return Task.FromResult(result);
        }

        // ===== Helpers =====

        private int? ExtractDeliveryDays(string text)
        {
            // أمثلة: "خلال 7 ايام" – "مدة التسليم 5 يوم" – "10 أيام"
            var m = Regex.Match(text, @"(\d+)\s*(يوم|ايام|أيام|يوما)");
            if (!m.Success)
                return null;

            if (int.TryParse(m.Groups[1].Value, out var days))
                return days;

            return null;
        }

        private List<int> ExtractAllNumbers(string text)
        {
            var list = new List<int>();

            var matches = Regex.Matches(text, @"\d+");
            foreach (Match m in matches)
            {
                if (int.TryParse(m.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
                    list.Add(n);
            }

            return list;
        }
    }
}
