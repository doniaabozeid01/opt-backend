using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.SupplierRate.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.SupplierRate
{
    public class RatingSupplierService : IRatingSupplierService
    {
        readonly IUnitOfWork _unitOfWork;
        public RatingSupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task RateSupplierAsync(SupplierRatingCreateDto dto)
        {
            var supplierRepo = _unitOfWork.Repository<Suppliers>();
            var ratingRepo = _unitOfWork.Repository<SupplierRating>();

            var supplier = await supplierRepo.GetByIdAsync(dto.SupplierId);
            if (supplier == null) throw new Exception("Supplier not found");

            // 1) نحفظ التقييم نفسه
            var rating = new SupplierRating
            {
                SupplierId = dto.SupplierId,
                SupplierRequestId = dto.SupplierRequestId,
                SupplierOfferId = dto.SupplierOfferId,
                Score = dto.Score,
                Comment = dto.Comment
            };
            await ratingRepo.AddAsync(rating);

            // 2) نحدّث المتوسط (AverageRating)
            var oldAvg = supplier.AverageRating ?? 0m;
            var oldCount = supplier.RatingCount;

            var newAvg = ((oldAvg * oldCount) + dto.Score) / (oldCount + 1);

            supplier.AverageRating = Math.Round(newAvg, 1); // 8.5 مثلاً
            supplier.RatingCount = oldCount + 1;

            supplierRepo.Update(supplier);
            await _unitOfWork.CompleteAsync();
        }

    }
}
