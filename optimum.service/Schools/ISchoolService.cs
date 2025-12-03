using Microsoft.AspNetCore.Identity;
using optimum.data.Entities;
using optimum.service.Schools.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Schools
{
    public interface ISchoolService
    {
        Task<School> CreateSchoolAsync(CreateSchoolDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteSchoolAsync(int id, CancellationToken cancellationToken = default);
        Task<School> GetSchoolByIdAsync(int id, CancellationToken cancellationToken = default);

        //Task<School> RegisterWithSchoolAsync(RegisterSchoolDto dto, CancellationToken cancellationToken = default);


        //Task<(School school, string token)> RegisterSchoolAsync(RegisterSchoolDto dto);
        Task<(School school, string token, IdentityResult identityResult)> RegisterSchoolAsync(RegisterSchoolDto dto);

    }
}
