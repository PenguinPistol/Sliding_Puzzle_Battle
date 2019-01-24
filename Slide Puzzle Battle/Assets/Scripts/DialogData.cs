using UnityEngine.Events;

public class DialogData
{
    private string message;
    private string positiveText;
    private string negativeText;
    private UnityAction positiveAction;
    private UnityAction negativeAction;

    public string Message { get { return message; } }
    public string PositiveText { get { return positiveText; } }
    public string NegativeText { get { return negativeText; } }
    public UnityAction PositiveAction { get { return positiveAction; } }
    public UnityAction NegativeAction { get { return negativeAction; } }

    private DialogData(Builder builder)
    {
        message = builder.Message;
        positiveText = builder.PositiveText;
        negativeText = builder.NegativeText;
        positiveAction = builder.PositiveAction;
    }

    public class Builder
    {
        private string message;
        private string positiveText;
        private string negativeText;
        private UnityAction positiveListener;
        private UnityAction negativeListener;

        public string Message { get { return message; } }
        public string PositiveText { get { return positiveText; } }
        public string NegativeText { get { return negativeText; } }
        public UnityAction PositiveAction { get { return positiveListener; } }
        public UnityAction NegativeAction { get { return negativeListener; } }

        public Builder(string _message)
        {
            message = _message;

            positiveText = "예";
            negativeText = "아니오";
        }

        public Builder SetPositiveAction(UnityAction _listener)
        {
            positiveListener = _listener;
            return this;
        }

        public Builder SetNegativeAction(UnityAction _listener)
        {
            negativeListener = _listener;
            return this;
        }

        public Builder SetPositiveText(string _text)
        {
            positiveText = _text;
            return this;
        }

        public Builder SetNegativeText(string _text)
        {
            negativeText = _text;
            return this;
        }

        public DialogData Build()
        {
            return new DialogData(this);
        }
    }
}
