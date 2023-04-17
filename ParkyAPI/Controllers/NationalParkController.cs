using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{nationalParkId:int}")] // must put the parameter into here, or the Request will not target this endpoint
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




    }
}
