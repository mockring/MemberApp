using Microsoft.AspNetCore.Mvc;
using MemerApp.Dtos;
using MemerApp.Services;

namespace MemerApp.Controllers
{
    public class ConsumptionController : Controller
    {
        private readonly IConsumptionService _service;

        public ConsumptionController(IConsumptionService service)
        {
            _service = service;
        }

        // GET: Consumption
        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        // GET: Consumption/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consumption = await _service.GetByIdAsync(id.Value);
            if (consumption == null) return NotFound();

            return View(consumption);
        }

        // GET: Consumption/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // 先取得要刪除的資料，呈現在確認頁面
            var consumption = await _service.GetByIdAsync(id.Value);
            if (consumption == null) return NotFound();

            return View(consumption);
        }

        // POST: Consumption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();   // 找不到或刪除失敗

            return RedirectToAction(nameof(Index));
        }

        // GET: Consumption/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dto = await _service.GetByIdAsync(id.Value);
            if (dto == null) return NotFound();

            return View(dto);
        }

        // POST: Consumption/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsumptionDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            // 重要：確保 DTO 裡的 Id 與 URL 裡的 id 一致
            dto.ConsumptionId = id;

            var success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                ModelState.AddModelError(string.Empty,
                    "Update failed – the phone number might already be in use.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Consumption/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 載入 coupons & products for select (frontend can also call AJAX)
            var coupons = await _service.GetAllCouponsAsync();
            ViewBag.Coupons = coupons;

            // 若你需要 products list:
            // var products = (await _service.GetAllProductsAsync()) ...
            // ViewBag.Products = products;

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCoupons()
        {
            var c = await _service.GetAllCouponsAsync();
            return Ok(c);
        }
        
        // POST: Consumption/Create  (expect JSON or form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ConsumptionDto dto)
        {
            if (dto == null) return BadRequest();

            // Server-side validation minimal
            if (string.IsNullOrWhiteSpace(dto.MemberPhone))
            {
                ModelState.AddModelError(nameof(dto.MemberPhone), "Member phone required");
                return BadRequest(ModelState);
            }
            if (dto.Lines == null || dto.Lines.Count == 0)
            {
                ModelState.AddModelError("Lines", "至少要有一筆商品");
                return BadRequest(ModelState);
            }

            var result = await _service.CreateConsumptionAsync(dto);

            // 回傳建立後結果（可改為 RedirectToAction 到 Details）
            return Ok(new { success = true, total = result.TotalAfterDiscount });
        }

        // AJAX: lookup member by phone
        [HttpGet]
        public async Task<IActionResult> GetMemberByPhone(string phone)
        {
            var m = await _service.FindMemberByPhoneAsync(phone);
            if (m == null) return NotFound();
            return Ok(m);
        }

        // AJAX: lookup product by keyword
        [HttpGet]                        // GET api/products/search?keyword=abc
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword)
        {
            // 參數檢查
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("keyword must not be empty");
            }

            // 執行服務層搜尋
            var product = await _service.GetProductsByKeywordAsync(keyword.Trim());

            // 回傳清單（即使為空也返回 200）
            return Ok(product);
        }

        // AJAX: lookup product by id
        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var p = await _service.GetProductByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }
    }
}