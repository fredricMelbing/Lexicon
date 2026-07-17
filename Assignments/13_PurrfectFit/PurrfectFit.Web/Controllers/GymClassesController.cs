using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurrfectFit.Core.Entities;
using PurrfectFit.Persistence.Data;
using PurrfectFit.Web.Models.ViewModels;

namespace PurrfectFit.Web.Controllers
{
	public class GymClassesController : Controller
	{
		private readonly ApplicationDbContext _context;
				
		public GymClassesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: GymClasses
		public async Task<IActionResult> Index()
		{			
			var gymClasses = await _context.GymClasses.ToListAsync();
			return View(gymClasses);
		}

		// GET: GymClasses/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
						
			var gymClass = await _context.GymClasses
				.FirstOrDefaultAsync(m => m.Id == id);

			if (gymClass == null)
			{
				return NotFound();
			}

			return View(gymClass);
		}

		// GET: GymClasses/Schedule
		public IActionResult Schedule()
		{			
			return View();
		}

		// POST: GymClasses/Schedule
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Schedule(GymClassScheduleViewModel model)
		{			
			if (ModelState.IsValid)
			{				
				var gymClass = new GymClass
				{
					Name = model.Name,
					StartTime = model.StartTime,
					Duration = model.Duration,
					Description = model.Description
				};
								
				_context.Add(gymClass);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}
						
			return View(model);
		}
		// GET: GymClasses/Edit/5
		public async Task<IActionResult> Reschedule(int? id)
		{
			if (id == null) return NotFound();

			var gymClass = await _context.GymClasses.FindAsync(id);
			if (gymClass == null) return NotFound();
						
			var model = new GymClassRescheduleViewModel
			{
				Id = gymClass.Id,
				Name = gymClass.Name,
				StartTime = gymClass.StartTime,
				Duration = gymClass.Duration,
				Description = gymClass.Description
			};

			return View(model);
		}

		// POST: GymClasses/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Reschedule(int id, GymClassRescheduleViewModel model)
		{
			if (id != model.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					var gymClass = await _context.GymClasses.FindAsync(id);
					if (gymClass == null) return NotFound();
										
					gymClass.Name = model.Name;
					gymClass.StartTime = model.StartTime;
					gymClass.Duration = model.Duration;
					gymClass.Description = model.Description;

					_context.Update(gymClass);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.GymClasses.Any(e => e.Id == model.Id)) return NotFound();
					throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}
		// GET: GymClasses/Cancel/5
		public async Task<IActionResult> Cancel(int? id)
		{
			if (id == null) return NotFound();

			var gymClass = await _context.GymClasses.FirstOrDefaultAsync(m => m.Id == id);
			if (gymClass == null) return NotFound();

			return View(gymClass);
		}

		// POST: GymClasses/Cancel/5
		[HttpPost, ActionName("Cancel")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CancelConfirmed(int id)
		{
			var gymClass = await _context.GymClasses.FindAsync(id);
			if (gymClass != null)
			{
				_context.GymClasses.Remove(gymClass);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
