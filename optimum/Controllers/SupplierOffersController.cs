using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.data.Enum;
using optimum.service.SupplierOffer.Dtos;
using optimum.service.SupplierOffer;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierOffersController : ControllerBase
    {

        private readonly ISupplierOfferService _supplierOfferService;

        public SupplierOffersController(ISupplierOfferService supplierOfferService)
        {
            _supplierOfferService = supplierOfferService;
        }

        // مؤقتًا:
        private int GetCurrentSupplierId() => 3; // بعدين هتجيبيه من الـ JWT

        // 1) Free Text
        // POST: api/SupplierOffers/{supplierRequestId}/free-text
        [HttpPost("{supplierRequestId:int}/free-text")]
        public async Task<IActionResult> SubmitFreeTextOffer(
    int supplierRequestId,
    [FromBody] SupplierFreeTextOfferCreateDto dto)
        {
            var supplierId = dto.SupplierId;

            var offer = await _supplierOfferService
                .SubmitFreeTextOfferAsync(supplierRequestId, supplierId, new SupplierFreeTextOfferCreateDto
                {
                    RawText = dto.RawText
                });

            var response = new
            {
                offer.Id,
                offer.SupplierRequestId,
                offer.OfferType,
                offer.RawText,
                offer.Status,
                offer.OfferDate
            };

            return Ok(response);
        }

        //[HttpPost("{supplierRequestId:int}/free-text")]
        //public async Task<IActionResult> SubmitFreeTextOffer(
        //    int supplierRequestId,
        //    [FromBody] SupplierFreeTextOfferCreateDto dto)
        //{
        //    //var supplierId = GetCurrentSupplierId();

        //    var offer = await _supplierOfferService
        //        .SubmitFreeTextOfferAsync(supplierRequestId, dto.SupplierId, dto);

        //    return Ok();
        //}

        // 2) File (PDF / Word / Excel / Image)
        // POST: api/SupplierOffers/{supplierRequestId}/file
        [HttpPost("{supplierRequestId:int}/file")]
        public async Task<IActionResult> SubmitFileOffer(
            int supplierRequestId,
            [FromForm] SupplierFileOfferCreateDto dto)
        {
            var supplierId = GetCurrentSupplierId();

            var offer = await _supplierOfferService
                .SubmitFileOrVoiceOfferAsync(supplierRequestId, supplierId, dto, SupplierOfferType.File);

            return Ok(offer);
        }

        // 3) Voice (Audio)
        // POST: api/SupplierOffers/{supplierRequestId}/voice
        [HttpPost("{supplierRequestId:int}/voice")]
        public async Task<IActionResult> SubmitVoiceOffer(
            int supplierRequestId,
            [FromForm] SupplierFileOfferCreateDto dto)
        {
            var supplierId = GetCurrentSupplierId();

            var offer = await _supplierOfferService
                .SubmitFileOrVoiceOfferAsync(supplierRequestId, supplierId, dto, SupplierOfferType.Voice);

            return Ok(offer);
        }

    }
}
