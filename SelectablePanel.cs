namespace ffmpegGui_SimpleCut;

class SelectablePanel : Panel
{
    public SelectablePanel()
    {
        SetStyle(ControlStyles.Selectable, true);
        TabStop = true;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Focus();
        base.OnMouseDown(e);
    }

    protected override bool IsInputKey(Keys keyData)
    {
        if(keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Space)
            return true;
        return base.IsInputKey(keyData);
    }

    protected override void OnEnter(EventArgs e)
    {
        Invalidate();
        base.OnEnter(e);
    }

    protected override void OnLeave(EventArgs e)
    {
        Invalidate();
        base.OnLeave(e);
    }
}
