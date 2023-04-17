﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")] // Here means we use Controller Name as URL, the Action Name will not be considered!!
    [ApiController]
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
        public IActionResult CreateNationalPrk([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);
            if (_parkRepository.NationalParkExists(nationalParkDto.Id))
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

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }


        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
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
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_parkRepository.NationalParkExists(nationalParkId)) return NotFound(ModelState);
            var nationalParkObj = _parkRepository.GetNationalPark(nationalParkId);



            if (!_parkRepository.DeleteNatinalPark(nationalParkObj!))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
