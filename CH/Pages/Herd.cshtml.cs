using CatHerder.Mediatr.Herd.Cat;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatHerder.Web.Pages
{
    public class HerdModel : PageModel
    {
        private readonly IMediator _mediator;

        public HerdModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public Guid PublicId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid VisitingCatId { get; set; }

        public CatHerder.HerdModel? Herd { get; set; }

        public async Task OnGet()
        {
            Herd = await _mediator.Send(new CatHerder.Mediatr.Herd.Get.Request { PublicId = PublicId });
        }

        public async Task<IActionResult> OnPost(string catId, string slotId, Response response)
        {
            await _mediator.Send(new CatHerder.Mediatr.Herd.Cat.AddEntry.Request() {
                Response= response,
                CatId= catId,
                SlotId= slotId
            });

            return RedirectToPage(new { PublicId, VisitingCatId });
        }
    }
}
