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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var purchasesResponse = await _purchasesService.GetAllAsync();

            var purchases = purchasesResponse.Content;

            var purchasesDto = _mapper.Map<List<PurchaseDto>>(purchases);

            return Ok(purchasesDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var purchaseResponse = await _purchasesService.GetAsync(id);

            if (!purchaseResponse.Success)
                return NotFound(purchaseResponse.Error);

            var purchase = purchaseResponse.Content;

            var purchaseDto = _mapper.Map<PurchaseDto>(purchase);

            return Ok(purchaseDto);
        }

        [Authorize]
        [HttpGet("current_user")]
        public async Task<IActionResult> GetAsync()
        {
            var purchasesResponse = await _purchasesService.GetAllAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (!purchasesResponse.Success)
                return NotFound(purchasesResponse.Error);

            var purchases = purchasesResponse.Content;

            var purchasesDto = _mapper.Map<List<PurchaseDto>>(purchases);

            return Ok(purchasesDto);
        }

        [Authorize]
        [HttpPost("current_user")]
        public async Task<IActionResult> SaveAsync(PurchaseUpsertDto purchaseDto)
        {
            var purchaseResponse = await _purchasesService.SaveAsync(purchaseDto.NumberOfTickets, Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value), purchaseDto.ShowId, purchaseDto.CardNumber, purchaseDto.ExpMonth, purchaseDto.ExpYear, purchaseDto.Cvc);

            if (!purchaseResponse.Success)
                return NotFound(purchaseResponse.Error);

            var savedPurchase = purchaseResponse.Content;

            var savedPurchaseDto = _mapper.Map<PurchaseDto>(savedPurchase);

            return Ok(savedPurchaseDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(PurchaseUpsertDto purchaseDto, int id)
        {
            var purchase = _mapper.Map<Purchase>(purchaseDto);
            purchase.Id = id;

            var purchaseResponse = await _purchasesService.UpdateAsync(purchase);

            if (!purchaseResponse.Success)
                return NotFound(purchaseResponse.Error);

            var updatedPurchase = purchaseResponse.Content;

            var updatedPurchaseDto = _mapper.Map<PurchaseDto>(updatedPurchase);

            return Ok(updatedPurchaseDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var purchaseResponse = await _purchasesService.DeleteAsync(id);

            if (!purchaseResponse.Success)
                return NotFound(purchaseResponse.Error);

            var deletedPurchase = purchaseResponse.Content;

            var deletedPurchaseDto = _mapper.Map<PurchaseDto>(deletedPurchase);

            return Ok(deletedPurchaseDto);
        }
    }
}
