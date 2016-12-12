namespace Bamboozle
{
	public class MessageContent
	{
		public string From { get; set; }
		public string Text { get; set; }

		public MessageContent() { }
		public MessageContent(string From, string Text)
		{
			this.From = From;
			this.Text = Text;
		}
	}
}