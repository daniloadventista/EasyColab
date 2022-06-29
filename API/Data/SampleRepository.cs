using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SampleRepository : ISampleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SampleRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddSample(Sample sample)
        {
            _context.Samples.Add(sample);
        }

        public void DeleteSample(Sample sample)
        {
            _context.Samples.Remove(sample);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Sample> GetSample(int id)
        {
            return await _context.Samples
                .Include(u => u.Sender)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

       public async Task<PagedList<SampleDto>> GetSamplesForUser(SampleParams sampleParams)
        {
            var query = _context.Samples
                .Where(s => s.Sender.UserName == sampleParams.Username)
                .OrderByDescending(s => s.Created)
                .ProjectTo<SampleDto>(_mapper.ConfigurationProvider)
                .AsQueryable();
           

            return await PagedList<SampleDto>.CreateAsync(query, sampleParams.PageNumber, sampleParams.PageSize);

        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
    }
}