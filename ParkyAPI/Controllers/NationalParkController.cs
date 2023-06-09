﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")] // Here means we use Controller Name as URL, the Action Name will not be considered!!
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalPark>))]
        public IActionResult GetNationalParks()
        {
            var objList = _parkRepository.GetNationalParks();
            // Never expose Domain model to outside world
            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")] // must put the parameter into here, or the Request will not target this endpoint
        [ProducesResponseType(200, Type = typeof(NationalPark))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _parkRepository.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalPark))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPrk([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);
            if (_parkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_parkRepository.CreateNatinalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkDto.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion()?.ToString(), nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id) return BadRequest(ModelState);
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_parkRepository.UpdateNatinalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkDto.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_parkRepository.NationalParkExists(nationalParkId)) return NotFound(ModelState);
            var nationalParkObj = _parkRepository.GetNationalPark(nationalParkId);

            if (!_parkRepository.DeleteNatinalPark(nationalParkObj!))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj?.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}