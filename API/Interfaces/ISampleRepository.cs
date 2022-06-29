using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ISampleRepository
    {
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        void AddSample(Sample sample);
        void DeleteSample(Sample sample);
        Task<Sample> GetSample(int id);
        Task<PagedList<SampleDto>> GetSamplesForUser(SampleParams sampleParams);
    }
}