namespace Bamboozle
{
	public class MessageContent
	{
		public string From { get; set; }
		public string Text { get; set; }
		public string Photo { get; set; }

		public MessageContent() { }
		public MessageContent(string From, string Text, string Photo)
		{
			this.From = From;
			this.Text = Text;
			this.Photo = Photo;
		}
	}
}