using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatHerder.Web.Pages
{
    public class CatModel : PageModel
    {
        private readonly IMediator? _mediator;

        public CatModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGet()
        {
            Cat = await _mediator!.Send(new CatHerder.Mediatr.Herd.Cat.Get.Request() { PublicId = PublicId });
        }

        [BindProperty(SupportsGet = true)]
        public Guid PublicId { get; set; }

        [BindProperty]
        public CatHerder.CatModel? Cat { get; set; }
    }
}
