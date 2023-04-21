using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Controllers
{ 
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();

            //Convert Domain to DTO
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
         public async Task<IActionResult> GetWalkDifficultyById(Guid id) //id Matches Route
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficutyRequest addWalkDifficultyRequest)
        {
            // Validate
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))  // ! means false - Validation failed
            {
                return BadRequest(ModelState);  //ModelState will show errors
            }

            //Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            //Call Repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert Domain to DTO uisng AutoMapper
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return Response
            return CreatedAtAction(nameof(GetWalkDifficultyById), 
                new { id = walkDifficultyDTO.Id}, walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id,
            Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))  // ! means false - Validation failed
            {
                return BadRequest(ModelState);  //ModelState will show errors
            }

            //Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //Pass Details to Repository - Get Domain Object in response (or null)
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            //Handle null (not found)
            if (walkDifficultyDomain == null)
            {
                return NotFound("Walk Difficulty Not Found");  //Displays the message
            }

            {
                //Convert Domain to DTO
                var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain); //Useing AutoMapper

          //      var walkDifficultyDTO = new Models.DTO.WalkDifficulty                          // Not using AutoMapper
          //      {
          //          Id = walkDifficultyDomain.Id,
          //          Code = walkDomain.Code
          //      };

                //Return response
                return Ok(walkDifficultyDTO);
            }
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //Call Repository to delete Walk
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficultyDomain == null)
            {
                return NotFound("Walk Difficulty not found");
            }

            // Convert to DTO using AutoMapper
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain); 

            return Ok(walkDifficultyDTO);
        }

        //Validations 

        #region Private Methods
        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficutyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                    $"Add Walk Difficulty Data Required.");
                //  return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} cannot be Null or Empty.");
                //   return false;
            }

            if (ModelState.ErrorCount > 0)  //Do this instead of return false on each check to accumulate all the errors
                {
                    return false;
                }

            return true;
        }
        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"Add Walk Difficulty Data Required.");
                //  return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} cannot be Null or Empty.");
                //   return false;
            }

            if (ModelState.ErrorCount > 0)  //Do this instead of return false on each check to accumulate all the errors
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
