namespace LibrarySystem.Dtos
{
    public class UserBorrowRequestDto
    {
        public int BookId { get; set; }
        public string UserId{ get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
