using MemerApp.Dtos;
using MemerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemerApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _service;

        public MemberController(IMemberService service)
        {
            _service = service;
        }

        // GET: Member
        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var member = await _service.GetByIdAsync(id.Value);
            if (member == null) return NotFound();

            return View(member); // 會傳入 MemberDto
        }

        // GET: Member/Create
        public IActionResult Create() => View();

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var success = await _service.CreateAsync(dto);
            if (!success)
            {
                ModelState.AddModelError(string.Empty,
                    "此電話號碼已被使用，請使用另一組電話號碼。");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dto = await _service.GetByIdAsync(id.Value);
            if (dto == null) return NotFound();

            return View(dto); // 會傳入 MemberDto
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            // 重要：確保 DTO 裡的 Id 與 URL 裡的 id 一致
            dto.MemberId = id;

            var success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                ModelState.AddModelError(string.Empty,
                    "Update failed – the phone number might already be in use.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // 先取得要刪除的資料，呈現在確認頁面
            var member = await _service.GetByIdAsync(id.Value);
            if (member == null) return NotFound();

            return View(member);   // 會把 MemberDto 傳給 Delete.cshtml
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();   // 找不到或刪除失敗

            return RedirectToAction(nameof(Index));
        }
    }
}
