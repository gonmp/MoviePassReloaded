using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Web.Dtos.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchasesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPurchasesService _purchasesService;

        public PurchasesController(IMapper mapper, IPurchasesService purchasesService)
        {
            _mapper = mapper;
            _purchasesService = purchasesService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("~/api/admin/purchases")]
        public async Task<IActionResult> GetAllAsync()
        {
            var purchases = await _purchasesService.GetAllAsync();

            var purchasesDto = _mapper.Map<List<PurchaseDTO>>(purchases);

            return Ok(purchasesDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("~/api/admin/purchases/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var purchase = await _purchasesService.GetAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PurchaseDTO>(purchase));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("~/api/user/purchases")]
        public async Task<IActionResult> GetAsync()
        {
            var purchase = await _purchasesService.GetAllAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<PurchaseDTO>>(purchase));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("~/api/user/purchases")]
        public async Task<IActionResult> SaveAsync(PurchaseUpsertDTO purchaseDto)
        {
            var purchase = await _purchasesService.SaveAsync(purchaseDto.NumberOfTickets, Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value), purchaseDto.ShowId, purchaseDto.CardNumber, purchaseDto.ExpMonth, purchaseDto.ExpYear, purchaseDto.Cvc);

            return Ok(_mapper.Map<PurchaseDTO>(purchase));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("~/api/admin/purchases/{id}")]
        public async Task<IActionResult> UpdateAsync(PurchaseUpsertDTO purchaseDto, int id)
        {
            var purchase = await _purchasesService.GetAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            _mapper.Map(purchaseDto, purchase);

            var updatedPurchase = _mapper.Map<PurchaseDTO>(await _purchasesService.UpdateAsync(purchase));

            return Ok(updatedPurchase);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("~/api/admin/purchases/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var purchase = await _purchasesService.GetAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            var deletedPurchase = _purchasesService.DeleteAsync(id);

            return Ok(_mapper.Map<PurchaseDTO>(deletedPurchase));
        }
    }
}
