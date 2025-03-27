using ICSharpCode.AvalonEdit;
using System.Windows;

namespace TestProjectForDCT.Helpers;

public static class AvalonTextEditorHelper
{
    public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached(
                "BindableText",
                typeof(string),
                typeof(AvalonTextEditorHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableTextChanged));

    public static string GetBindableText(DependencyObject obj)
    {
        return (string)obj.GetValue(BindableTextProperty);
    }

    public static void SetBindableText(DependencyObject obj, string value)
    {
        obj.SetValue(BindableTextProperty, value);
    }

    private static void OnBindableTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var textEditor = d as TextEditor;
        if (textEditor == null) return;

        textEditor.TextChanged -= TextEditorOnTextChanged;

        var newText = e.NewValue as string;
        if (textEditor.Text != newText)
        {
            textEditor.Text = newText;
        }

        textEditor.TextChanged += TextEditorOnTextChanged;
    }

    private static void TextEditorOnTextChanged(object sender, EventArgs e)
    {
        var textEditor = sender as TextEditor;
        if (textEditor == null) return;

        SetBindableText(textEditor, textEditor.Text);
    }
}
