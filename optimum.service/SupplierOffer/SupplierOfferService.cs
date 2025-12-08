using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using optimum.data.Entities;
using optimum.data.Enum;
using optimum.repository.Interfaces;
using optimum.service.SupplierOffer.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace optimum.service.SupplierOffer
{
    public class SupplierOfferService : ISupplierOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _env;

        public SupplierOfferService(IUnitOfWork unitOfWork, IHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        // ============= 1) Free Text Offer =============
        public async Task<SupplierOffers> SubmitFreeTextOfferAsync(
            int supplierRequestId,
            int supplierId,
            SupplierFreeTextOfferCreateDto dto)
        {
            var supplierRequestRepo = _unitOfWork.Repository<SupplierRequest>();

            var supplierRequest = await supplierRequestRepo
                .GetTable()
                .FirstOrDefaultAsync(sr => sr.Id == supplierRequestId);

            if (supplierRequest == null)
                throw new Exception("SupplierRequest not found");

            if (supplierRequest.SupplierId != supplierId)
                throw new Exception("Not allowed to submit offer for this request.");

            var offerRepo = _unitOfWork.Repository<SupplierOffers>();

            var offer = new SupplierOffers
            {
                SupplierRequestId = supplierRequestId,
                OfferType = SupplierOfferType.FreeText,
                RawText = dto.RawText,
                Status = SupplierOfferStatus.PendingAI,
                OfferDate = DateTime.UtcNow,

            };

            await offerRepo.AddAsync(offer);
            await _unitOfWork.CompleteAsync();

            // هنا في step 1 مش هنعمل AI ولا حاجة
            return offer;
        }

        // ============= 2) File or Voice Offer =============
        public async Task<SupplierOffers> SubmitFileOrVoiceOfferAsync(
            int supplierRequestId,
            int supplierId,
            SupplierFileOfferCreateDto dto,
            SupplierOfferType type)
        {
            if (dto.File == null || dto.File.Length == 0)
                throw new Exception("File is required");

            var supplierRequestRepo = _unitOfWork.Repository<SupplierRequest>();

            var supplierRequest = await supplierRequestRepo
                .GetTable()
                .FirstOrDefaultAsync(sr => sr.Id == supplierRequestId);

            if (supplierRequest == null)
                throw new Exception("SupplierRequest not found");

            if (supplierRequest.SupplierId != supplierId)
                throw new Exception("Not allowed to submit offer for this request.");

            // 1) حفظ الملف
            //var uploadsRoot = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "uploads", "offers");
            var uploadsRoot = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "offers");
            // أو لو عايزة من غير wwwroot:
            // var uploadsRoot = Path.Combine(_env.ContentRootPath, "uploads", "offers");
            //Directory.CreateDirectory(uploadsRoot);

            Directory.CreateDirectory(uploadsRoot);

            var uniqueFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsRoot, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // 2) إنشاء العرض
            var offerRepo = _unitOfWork.Repository<SupplierOffers>();

            var offer = new SupplierOffers
            {
                SupplierRequestId = supplierRequestId,
                OfferType = type,                      // File أو Voice
                FileName = dto.File.FileName,
                FilePath = $"/uploads/offers/{uniqueFileName}",
                ContentType = dto.File.ContentType,
                Status = SupplierOfferStatus.PendingAI,
                OfferDate = DateTime.UtcNow
            };

            await offerRepo.AddAsync(offer);
            await _unitOfWork.CompleteAsync();

            return offer;
        }
    }
}
