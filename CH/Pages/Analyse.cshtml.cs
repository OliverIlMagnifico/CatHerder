using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatHerder.Web.Pages
{
    public class AnalyseModel : PageModel
    {
        private readonly IMediator _mediator;

        public AnalyseModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public Guid PublicId { get; set; }

        [BindProperty]
        public ISlot[]? BestSlots { get; set; }

        public async Task OnGet()
        {
            BestSlots = await _mediator.Send(new CatHerder.Mediatr.Herd.Analyse.Request(){ PublicId = PublicId });
        }
    }
}
