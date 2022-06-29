using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SamplesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SamplesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SampleDto>>> GetSamplesForUser([FromQuery]
            SampleParams sampleParams)
        {
            sampleParams.Username = User.GetUsername();

            var samples = await _unitOfWork.SampleRepository.GetSamplesForUser(sampleParams);

            Response.AddPaginationHeader(samples.CurrentPage, samples.PageSize,
                samples.TotalCount, samples.TotalPages);

            return samples;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSample(int id)
        {
            var username = User.GetUsername();

            var sample = await _unitOfWork.SampleRepository.GetSample(id);

            if (sample.Sender.UserName != username)
                return Unauthorized();

            if (sample.Sender.UserName == username) sample.SenderDeleted = true;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the sample");
        }
    }
}