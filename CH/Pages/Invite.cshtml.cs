using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatHerder.Web.Pages
{
    public class InviteModel : PageModel
    {
        private readonly IMediator _mediator;

        public InviteModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public Guid PublicId { get; set; }

        [BindProperty]
        public string? Name { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var catId = await _mediator.Send(new CatHerder.Mediatr.Herd.AddCat.Request() { Name = Name, HerdPublicId = PublicId });

            return RedirectToPage("/Herd", new { PublicId, VisitingCatId = catId });
        }
    }
}
