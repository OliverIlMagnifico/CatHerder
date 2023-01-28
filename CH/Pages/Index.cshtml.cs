using CatHerder;
using CatHerder.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CH.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public string? Name { get; set; }
        [BindProperty]

        public string? FirstCat { get; set; }
        [BindProperty]
        public int Months { get; set; }
        [BindProperty]
        public bool Sunday { get; set; }
        [BindProperty]
        public bool Monday { get; set; }
        [BindProperty]
        public bool Tuesday { get; set; }
        [BindProperty]
        public bool Wednesday { get; set; }
        [BindProperty]
        public bool Thursday { get; set; }
        [BindProperty]
        public bool Friday { get; set; }
        [BindProperty]
        public bool Saturday { get; set; }

        public List<SelectListItem> MonthOptions => new List<SelectListItem> { new SelectListItem("1", "1"), new SelectListItem("2", "2"), new SelectListItem("3", "3") };


        public async Task<IActionResult> OnPost()
        {
            var result = await _mediator.Send(new CatHerder.Mediatr.Herd.Create.Request() {
                Name = Name,
                FirstCat= FirstCat,
                Saturday = Saturday,
                Monday = Monday,
                Tuesday = Tuesday,
                Wednesday = Wednesday,
                Thursday = Thursday,
                Friday = Friday,
                Sunday= Sunday,
                Months= Months
            });
            return RedirectToPage("/Herd", new { PublicId = result.herdId, VisitingCatId = result.catId });
        }
    }
}