using MemerApp.Dtos;
using MemerApp.Models;
using MemerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MemerApp.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
        }

        // GET: Coupon
        public async Task<IActionResult> Index()
        {
            IEnumerable<CouponDto> coupons = await _service.GetAllAsync();
            return View(coupons);
        }

        // GET: Coupon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var coupon = await _service.GetByIdAsync(id.Value);
            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // GET: Coupon/Create
        public IActionResult Create()
        {
            ViewData["CalculationMethodList"] = GetCalculationMethodList();
            return View();
        }

        // POST: Coupon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Coupon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var coupon = await _service.GetByIdAsync(id.Value);
            if (coupon == null) return NotFound();

            ViewData["CalculationMethodList"] = GetCalculationMethodList();
            return View(coupon);
        }

        // POST: Coupon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CouponId,CouponName,CalculationMethod,DiscountValue,Remark")] CouponDto dto)
        {
            if (id != dto.CouponId) return NotFound();

            if (!ModelState.IsValid) return View(dto);

            try
            {
                await _service.UpdateAsync(dto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _service.ExistsAsync(dto.CouponId))
                    return NotFound();
                throw;   // 或自行回傳錯誤訊息
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Coupon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var coupon = await _service.GetByIdAsync(id.Value);
            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // POST: Coupon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> GetCalculationMethodList()
        {
            return Enum.GetValues(typeof(CalculationMethod))
                       .Cast<CalculationMethod>()
                       .Select(e => new SelectListItem
                       {
                           Value = e.ToString(),
                           Text = e.ToString()
                       });
        }
    }
}