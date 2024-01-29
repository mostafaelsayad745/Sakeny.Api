using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sakeny.DbContexts;
using sakeny.Models;
using sakeny.Services;

namespace sakeny.Controllers
{
    [Route("api/users/{userid}/postfeedbacks")]
    [ApiController]
    public class PostFeedbackController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;

        public PostFeedbackController(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetPostFeedbacks(int userId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var postFeedbacksForUser = await _userInfoRepository.GetPostFeedbacksForUserAsync(userId);
            return Ok(_mapper.Map<IEnumerable<PostFeedbackForReturnDto>>(postFeedbacksForUser));
        }

        [HttpGet("{feedbackid}",Name = "GetPostFeedback")]
        public async Task<IActionResult> GetPostFeedback(int userId, int feedbackId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var postFeedbackForUser = await _userInfoRepository.GetPostFeedbackForUserAsync(userId, feedbackId);

            if (postFeedbackForUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostFeedbackForReturnDto>(postFeedbackForUser));
        }


        [HttpPost ]
        public async Task<IActionResult> AddPostFeedback(int userId, PostFeedbackForCreationDto postFeedbackForCreationDto)
        {
            if(! await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var postFeedbackEntity = _mapper.Map<Entities.PostFeedbackTbl>(postFeedbackForCreationDto);
            await _userInfoRepository.AddPostFeedbackForUserAsync(userId, postFeedbackEntity);
            await _userInfoRepository.SaveChangesAsync();
            var postFeedbackToReturn = _mapper.Map<PostFeedbackForReturnDto>(postFeedbackEntity);
            return CreatedAtRoute("GetPostFeedback", new { userId = userId, feedbackId = postFeedbackToReturn.PostFeedId }, postFeedbackToReturn);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePostFeedback(int userId, int feedbackId, PostFeedbackForUpdateDto postFeedbackForUpdateDto)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(postFeedbackForUpdateDto, postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{feedbackId}")]
        public async Task<IActionResult> PartiallyUpdatePostFeedback(int userId, int feedbackId, JsonPatchDocument<PostFeedbackForUpdateDto> patchDocument)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            var postFeedbackToPatch = _mapper.Map<PostFeedbackForUpdateDto>(postFeedbackFromRepo);
            patchDocument.ApplyTo(postFeedbackToPatch, ModelState);
            if (!TryValidateModel(postFeedbackToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(postFeedbackToPatch, postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeletePostFeedback(int userId, int feedbackId)
        {
            if (!await _userInfoRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var postFeedbackFromRepo = await _userInfoRepository.GetPostFeedbackForUserAsync(userId, feedbackId);
            if (postFeedbackFromRepo == null)
            {
                return NotFound();
            }
            _userInfoRepository.DeletePostFeedback(postFeedbackFromRepo);
            await _userInfoRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
