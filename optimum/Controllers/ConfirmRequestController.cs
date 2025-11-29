using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.ConfirmParsedItemsRequest.Dtos;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{requestId}/confirmed-items")]
        public async Task<IActionResult> GetConfirmedItems(int requestId)
        {
            // 1) هات كل العناصر المؤكدة من الريبو
            var repo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();
            var allItems = await repo.GetAllAsync();

            // 2) فلتر على الـ requestId في الذاكرة
            var items = allItems
                .Where(i => i.SchoolRequestId == requestId)
                .ToList();

            // 3) حوّلهم لـ DTO للـ UI
            var dto = items.Select(i => new SchoolConfirmedItemDto
            {
                Id = i.Id,
                SchoolRequestId = i.SchoolRequestId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Notes = i.Notes,
                IsConfirmed = i.IsConfirmed
            }).ToList();

            return Ok(dto);
        }





        [HttpGet("by-school/{schoolId}")]
        public async Task<IActionResult> GetConfirmedItemsBySchool(int schoolId)
        {
            var repo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();

            // هات كل العناصر:
            var allItems = await repo.GetAllAsync();

            // فلتر حسب الـ SchoolId الخاص بالطلب:
            var items = allItems
                .Where(i => i.SchoolRequest != null && i.SchoolRequest.SchoolId == schoolId)
                .ToList();

            // mapping
            var dto = items.Select(i => new SchoolConfirmedItemDto
            {
                Id = i.Id,
                SchoolRequestId = i.SchoolRequestId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Notes = i.Notes,
                IsConfirmed = i.IsConfirmed
            }).ToList();

            return Ok(dto);
        }









        //[HttpPut("confirmed-items")]
        //public async Task<IActionResult> UpdateConfirmedItems(
        //    [FromBody] List<UpdateConfirmedItemDto> items)
        //{
        //    if (items == null || !items.Any())
        //        return BadRequest("No items to update");

        //    var repo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();

        //    foreach (var item in items)
        //    {
        //        var entity = await repo.GetByIdAsync(item.Id);
        //        if (entity == null) continue;

        //        entity.ProductId = item.ProductId??0;
        //        entity.ProductName = item.ProductName;
        //        entity.Quantity = item.Quantity;
        //        entity.Notes = item.Notes;
        //        entity.IsConfirmed = item.IsConfirmed;
        //        entity.ConfirmedAt = DateTime.UtcNow;

        //        repo.Update(entity);
        //    }

        //    await _unitOfWork.CompleteAsync();

        //    return Ok(new { message = "Items updated successfully" });
        //}






















        //    [HttpPut("confirmed-items/{requestId}")]
        //    public async Task<IActionResult> UpdateConfirmedItems(
        //int requestId,
        //[FromBody] List<UpdateConfirmedItemDto> items)
        //    {
        //        if (items == null || !items.Any())
        //            return BadRequest("No items provided");

        //        var repo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();

        //        // 1) احذف القديم بالكامل
        //        var oldItems = await repo.GetAllAsync();
        //        var toDelete = oldItems.Where(i => i.SchoolRequestId == requestId).ToList();

        //        foreach (var old in toDelete)
        //            repo.Delete(old);

        //        // 2) أضف الجديد بالكامل
        //        foreach (var item in items)
        //        {
        //            var newItem = new SchoolConfirmedRequestItems
        //            {
        //                SchoolRequestId = requestId,
        //                ProductId = item.ProductId ?? 0,
        //                ProductName = item.ProductName,
        //                Quantity = item.Quantity,
        //                Notes = item.Notes,
        //                IsConfirmed = item.IsConfirmed,
        //                ConfirmedAt = DateTime.UtcNow
        //            };

        //            await repo.AddAsync(newItem);
        //        }

        //        await _unitOfWork.CompleteAsync();

        //        return Ok(new { message = "Items updated successfully" });
        //    }



















        [HttpPut("confirmed-items/{requestId}")]
        public async Task<IActionResult> UpdateConfirmedItems(
    int requestId,
    [FromBody] List<UpdateConfirmedItemDto> items)
        {
            if (items == null || !items.Any())
                return BadRequest("No items provided");

            var repo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();

            // 1) احذف القديم بالكامل
            var oldItems = await repo.GetAllAsync();
            var toDelete = oldItems.Where(i => i.SchoolRequestId == requestId).ToList();

            foreach (var old in toDelete)
                repo.Delete(old);

            // 2) أضف الجديد بالكامل
            foreach (var item in items)
            {
                var newItem = new SchoolConfirmedRequestItems
                {
                    SchoolRequestId = requestId,
                    ProductId = item.ProductId ?? 0,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    IsConfirmed = item.IsConfirmed,
                    ConfirmedAt = DateTime.UtcNow
                };

                await repo.AddAsync(newItem);
            }

            await _unitOfWork.CompleteAsync();

            // 3) بعد ما المدرسة تأكد البنود → ابعت للموردين
            await SendConfirmedRequestToSuppliersAsync(requestId);

            return Ok(new { message = "Items updated and sent to suppliers successfully" });
        }

















        //private async Task SendConfirmedRequestToSuppliersAsync(int requestId)
        //{
        //    // 1) هات البنود المؤكدة للطلب
        //    var confirmedRepo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();
        //    var allConfirmed = await confirmedRepo.GetAllAsync();

        //    var confirmedItems = allConfirmed
        //        .Where(i => i.SchoolRequestId == requestId
        //                    && i.IsConfirmed
        //                    && i.ProductId != 0)
        //        .ToList();

        //    if (!confirmedItems.Any())
        //        return; // مفيش حاجة مؤكدة، خلاص

        //    var productIds = confirmedItems
        //        .Select(i => i.ProductId)
        //        .Distinct()
        //        .ToList();

        //    // 2) هات الموردين اللي بيبيعوا المنتجات دي
        //    var supplierProductsRepo = _unitOfWork.Repository<SupplierProducts>();
        //    var allSupplierProducts = await supplierProductsRepo.GetAllAsync();

        //    var relatedSupplierProducts = allSupplierProducts
        //        .Where(sp => productIds.Contains(sp.ProductsId))
        //        .ToList();

        //    var supplierIds = relatedSupplierProducts
        //        .Select(sp => sp.SuppliersId)
        //        .Distinct()
        //        .ToList();

        //    if (!supplierIds.Any())
        //        return; // مفيش موردين مناسبين (ممكن لاحقاً نعمل Log)

        //    var supplierRequestRepo = _unitOfWork.Repository<SupplierRequest>();
        //    var existingSupplierRequests = await supplierRequestRepo.GetAllAsync();

        //    // اختياري: امسح أي SupplierRequest قديم لنفس الطلب عشان ما يتكررش
        //    var oldRequests = existingSupplierRequests
        //        .Where(sr => sr.SchoolRequestId == requestId)
        //        .ToList();

        //    foreach (var old in oldRequests)
        //        supplierRequestRepo.Delete(old);

        //    // 3) أنشئ SupplierRequest لكل مورد يقدر يوفّر حاجة من الطلب
        //    foreach (var supplierId in supplierIds)
        //    {
        //        var sr = new SupplierRequest
        //        {
        //            SupplierId = supplierId,
        //            SchoolRequestId = requestId,
        //            SentAt = DateTime.UtcNow
        //        };

        //        await supplierRequestRepo.AddAsync(sr);
        //    }

        //    await _unitOfWork.CompleteAsync();
        //}














        private async Task SendConfirmedRequestToSuppliersAsync(int requestId)
        {
            // 1) هات البنود المؤكدة للطلب
            var confirmedRepo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();
            var allConfirmed = await confirmedRepo.GetAllAsync();

            var confirmedItems = allConfirmed
                .Where(i => i.SchoolRequestId == requestId
                            && i.IsConfirmed
                            && i.ProductId != 0)
                .ToList();

            if (!confirmedItems.Any())
                return; // مفيش حاجة مؤكدة، خلاص

            var productIds = confirmedItems
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            // 2) هات الموردين اللي بيبيعوا المنتجات دي
            var supplierProductsRepo = _unitOfWork.Repository<SupplierProducts>();
            var allSupplierProducts = await supplierProductsRepo.GetAllAsync();

            var relatedSupplierProducts = allSupplierProducts
                .Where(sp => productIds.Contains(sp.ProductsId))   // ProductsId على حسب اسم العمود عندك
                .ToList();

            if (!relatedSupplierProducts.Any())
                return; // مفيش موردين مناسبين

            var supplierRequestRepo = _unitOfWork.Repository<SupplierRequest>();
            var supplierRequestItemRepo = _unitOfWork.Repository<SupplierRequestItem>();

            var existingSupplierRequests = await supplierRequestRepo.GetAllAsync();
            var existingItems = await supplierRequestItemRepo.GetAllAsync();

            // 3) امسح أي SupplierRequest + Items قديمة لنفس الطلب (لو عايزة إعادة إرسال من الأول)
            var oldRequests = existingSupplierRequests
                .Where(sr => sr.SchoolRequestId == requestId)
                .ToList();

            foreach (var old in oldRequests)
            {
                // امسح الـ items المرتبطة بيه
                var oldItemsForRequest = existingItems
                    .Where(it => it.SupplierRequestId == old.Id)
                    .ToList();

                foreach (var oi in oldItemsForRequest)
                    supplierRequestItemRepo.Delete(oi);

                supplierRequestRepo.Delete(old);
            }

            // 4) Group by Supplier → كل مورد له SupplierRequest واحد
            var supplierGroups = relatedSupplierProducts
                .GroupBy(sp => sp.SuppliersId)
                .ToList();

            foreach (var group in supplierGroups)
            {
                int supplierId = group.Key;

                // أنشئ SupplierRequest جديد للمورد ده
                var sr = new SupplierRequest
                {
                    SupplierId = supplierId,
                    SchoolRequestId = requestId,
                    Status = 0, // Sent
                    SentAt = DateTime.UtcNow
                };

                await supplierRequestRepo.AddAsync(sr);

                // 5) أنشئ SupplierRequestItem لكل بند confirmed يخص هذا المورد
                foreach (var ci in confirmedItems)
                {
                    // المورد ده عنده المنتج ده؟
                    bool supplierHasProduct = group.Any(sp => sp.ProductsId == ci.ProductId);
                    if (!supplierHasProduct)
                        continue;

                    var sri = new SupplierRequestItem
                    {
                        SupplierRequest = sr,                  // EF هيملي SupplierRequestId
                        SchoolConfirmedRequestItemId = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.ProductName,
                        Quantity = ci.Quantity,
                        Notes = ci.Notes,
                        AddedAt = DateTime.UtcNow
                    };

                    await supplierRequestItemRepo.AddAsync(sri);
                }
            }

            await _unitOfWork.CompleteAsync();
        }








    }
}
