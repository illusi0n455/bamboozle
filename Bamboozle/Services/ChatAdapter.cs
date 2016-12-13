using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
namespace Bamboozle
{
	class ChatAdapter : ArrayAdapter<ChatContent>
	{
		private List<ChatContent> _chatList;
		private Activity _context;
		public ChatAdapter(Activity context, List<ChatContent> chatList)
			: base(context, Resource.Layout.ChatBubble, chatList)
		{
			this._context = context;
			this._chatList = chatList;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = this._chatList[position];
			var view = (convertView ??
				   this._context.LayoutInflater.Inflate(
				   Resource.Layout.ChatItem,
				   parent,
				   false)) as LinearLayout;

			TextView txtTitle = view.FindViewById<TextView>(Resource.Id.txtTitle);
			txtTitle.Text = item.Title;
			return view;
		}
	}
}