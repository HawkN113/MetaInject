using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class NotesService: INotesService
{
    public string CurrentInfo()=> "Notes service info";
}