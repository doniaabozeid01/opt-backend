using optimum.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.TextRequestsParser
{
    public interface ITextRequestParserService
    {
        Task<List<AIParsedRequestItems>> ParseAsync(SchoolRequests request);

        Task<(int? productId, double confidence)> PredictProductIdFromNameAsync(string productText);


    }
}
