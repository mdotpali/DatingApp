using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{        [Authorize]

    public class MembersController(IUnitOfWork uow, IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Member>>> GetAllUsers([FromQuery] MemberParams memberParams)
        {
            memberParams.CurrentMemberId = User.GetMemberId();
            
            return Ok(await uow.MemberRepository.GetAllMembersAsync(memberParams));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetUserById(string id)
        {
            var member = await uow.MemberRepository.GetMemberByIdAsync(id);
            
            if(member == null)
                return NotFound();
            
            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await uow.MemberRepository.GetPhotsForMemberAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();
            
            var member = await uow.MemberRepository.GetMemberForUpdatesAsync(memberId);
            if(member is null) return BadRequest("Could not get member");

            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;
            
            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;
            
            uow.MemberRepository.Update(member);

            if (await uow.Complete()) return NoContent();

            return BadRequest("Failed to update member");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member = await uow.MemberRepository.GetMemberForUpdatesAsync(User.GetMemberId());
            if(member == null) return BadRequest("Cannot update member");
            
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };
            
            member.Photos.Add(photo);
            if(await uow.Complete()) return photo;
            return BadRequest("Failed to add photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await uow.MemberRepository.GetMemberForUpdatesAsync(User.GetMemberId());

            if (member == null) return BadRequest("Cannot get member from token");
            
            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Cannot set this as main image");
            }
            
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
            
            if(await uow.Complete()) return NoContent();

            return BadRequest("Problem setting main Photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await uow.MemberRepository.GetMemberForUpdatesAsync(User.GetMemberId());

            if (member == null) return BadRequest("Cannot get member from token"); 
            
            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("Cannot delete this photo");
            }

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            if (await uow.Complete()) return Ok();

            return BadRequest("Problem with Deleting Photo");
        }
    }
}
