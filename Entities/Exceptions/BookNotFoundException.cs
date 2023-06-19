namespace Entities.Exceptions
{
    //sealed kalıtım alınamayan class türüdür
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(int id) : base($"The book with id: {id} could not found.")
        {
        }
    }
}
