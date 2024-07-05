using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ViewModelsSamples.Axes.Logarithmic;

public partial class ViewModel : ObservableObject
{
    // base 10 log, change the base if you require it.
    // or use any custom scale the logic is the same.
    private static readonly int s_logBase = 10;
    private LvcPointD _crossPointD;
    public RelayCommand<PointerCommandArgs> PointerMoveCmd { get; }
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<LogarithmicPoint>
        {
            // for the x coordinate, we use the X property
            // and for the Y coordinate, we will map it to the logarithm of the value
            Mapping = (logPoint, index) => new(Math.Log(logPoint.X, s_logBase), Math.Log(logPoint.Y, s_logBase)),
            Values = new LogarithmicPoint[]
            {
                new() { X = 1, Y = 1 },
                new() { X = 10, Y = 10 },
                new() { X = 100, Y = 100 },
                new() { X = 1000, Y = 1000 },
                new() { X = 10000, Y = 10000 },
                new() { X = 100000, Y = 100000 },
                new() { X = 1000000, Y = 1000000 },
                new() { X = 10000000, Y = 10000000 }
            }
        }
    };

    public Axis[] XAxes { get; set; } =
    {
        new LogaritmicAxis(s_logBase)
        {
            SeparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(100),
                StrokeThickness = 1,
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(50),
                StrokeThickness = 0.5f
            },
            SubseparatorsCount = 9,
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            CrosshairPadding = new Padding(30,0, 0, 30),
            SubticksPaint = new SolidColorPaint(SKColors.Gray, 1),
        }
    };

    public Axis[] YAxes { get; set; } =
    {
        new LogaritmicAxis(s_logBase)
        {
            SeparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(100),
                StrokeThickness = 1,
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(50),
                StrokeThickness = 0.5f
            },
            SubseparatorsCount = 9,
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            CrosshairPadding = new Padding(30,0, 0, 0),
            SubticksPaint = new SolidColorPaint(SKColors.Gray, 1),
            //TicksPaint = new SolidColorPaint(SKColors.Gray, 1),
            //TicksAtCenter = true
            //DrawTicksPath = true
        }
    };
    public ViewModel()
    {
        _crossPointD = new LvcPointD();
        PointerMoveCmd = new RelayCommand<PointerCommandArgs>(PointerMove);
    }

    private string ToSuperscript(double value)
    {
        var tt = Math.Pow(10, value);
        var t1 = Math.Floor(Math.Log10(tt)).ToString(CultureInfo.InvariantCulture);
        if (Math.Abs(tt - _crossPointD.Y) <= 0)
            return tt.ToString("0.000");
        //Debug.Print($"To {tt} E{t1}");
        var superscriptDigits = new Dictionary<char, char>
        {
            { '0', '⁰' }, { '1', '¹' }, { '2', '²' }, { '3', '³' },
            { '4', '⁴' }, { '5', '⁵' }, { '6', '⁶' }, { '7', '⁷' },
            { '8', '⁸' }, { '9', '⁹' }, { '-', '⁻' }
        };
        var exponet = "";
        foreach (var c in t1.ToCharArray())
        {
            if (!superscriptDigits.ContainsKey(c)) continue;
            exponet += superscriptDigits[c];
        }
        return $"10{exponet}";
    }
    private void PointerMove(PointerCommandArgs? e)
    {
        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)e?.Chart!;
        var p = chart.ScalePixelsToData(e.PointerPosition);
        _crossPointD = new LvcPointD(p.X, Math.Pow(10, p.Y));
        Debug.Print($"Move {e.PointerPosition.X},{e.PointerPosition.Y} {p.X},{p.Y}");
    }
}

public partial class ViewModelX : ObservableObject
{
    // base 10 log, change the base if you require it.
    // or use any custom scale the logic is the same.
    private static readonly int s_logBase = 10;
    private LvcPointD _crossPointD;
    public RelayCommand<PointerCommandArgs> PointerMoveCmd { get; }
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<LogarithmicPoint>
        {
            // for the x coordinate, we use the X property
            // and for the Y coordinate, we will map it to the logarithm of the value
            Mapping = (logPoint, index) => new(Math.Log(logPoint.X, s_logBase), logPoint.Y),
            Values = new LogarithmicPoint[]
            {
                new() { X = 1, Y = 1 },
                new() { X = 10, Y = 2 },
                new() { X = 100, Y = 3 },
                new() { X = 1000, Y = 4 },
                new() { X = 10000, Y = 5 },
                new() { X = 100000, Y = 6 },
                new() { X = 1000000, Y = 7 },
                new() { X = 10000000, Y = 8 }
            }
        }
    };

    public Axis[] XAxes { get; set; } =
    {
        new LogaritmicAxis(s_logBase)
        {
            SeparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(100),
                StrokeThickness = 1,
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(50),
                StrokeThickness = 0.5f
            },
            SubseparatorsCount = 9,
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            CrosshairPadding = new Padding(30,0, 0, 30),
        }
    };

    public ViewModelX()
    {
        _crossPointD = new LvcPointD();
        PointerMoveCmd = new RelayCommand<PointerCommandArgs>(PointerMove);
    }

    private string ToSuperscript(double value)
    {
        var tt = Math.Pow(10, value);
        var t1 = Math.Floor(Math.Log10(tt)).ToString(CultureInfo.InvariantCulture);
        if (Math.Abs(tt - _crossPointD.Y) <= 0)
            return tt.ToString("0.000");
        //Debug.Print($"To {tt} E{t1}");
        var superscriptDigits = new Dictionary<char, char>
        {
            { '0', '⁰' }, { '1', '¹' }, { '2', '²' }, { '3', '³' },
            { '4', '⁴' }, { '5', '⁵' }, { '6', '⁶' }, { '7', '⁷' },
            { '8', '⁸' }, { '9', '⁹' }, { '-', '⁻' }
        };
        var exponet = "";
        foreach (var c in t1.ToCharArray())
        {
            if (!superscriptDigits.ContainsKey(c)) continue;
            exponet += superscriptDigits[c];
        }
        return $"10{exponet}";
    }
    private void PointerMove(PointerCommandArgs? e)
    {
        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)e?.Chart!;
        var p = chart.ScalePixelsToData(e.PointerPosition);
        _crossPointD = new LvcPointD(p.X, Math.Pow(10, p.Y));
        Debug.Print($"Move {e.PointerPosition.X},{e.PointerPosition.Y} {p.X},{p.Y}");
    }
}

public partial class ViewModelY : ObservableObject
{
    // base 10 log, change the base if you require it.
    // or use any custom scale the logic is the same.
    private static readonly int s_logBase = 10;
    private LvcPointD _crossPointD;
    public RelayCommand<PointerCommandArgs> PointerMoveCmd { get; }
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<LogarithmicPoint>
        {
            // for the x coordinate, we use the X property
            // and for the Y coordinate, we will map it to the logarithm of the value
            Mapping = (logPoint, index) => new(logPoint.X, Math.Log(logPoint.Y, s_logBase)),
            Values = new LogarithmicPoint[]
            {
                new() { X = 1, Y = 1 },
                new() { X = 2, Y = 10 },
                new() { X = 3, Y = 100 },
                new() { X = 4, Y = 1000 },
                new() { X = 5, Y = 10000 },
                new() { X = 6, Y = 100000 },
                new() { X = 7, Y = 1000000 },
                new() { X = 8, Y = 10000000 }
            }

        }
    };

    public Axis[] YAxes { get; set; } =
    {
        new LogaritmicAxis(s_logBase)
        {
            SeparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(100),
                StrokeThickness = 1,
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(50),
                StrokeThickness = 0.5f
            },
            SubseparatorsCount = 9,
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            CrosshairPadding = new Padding(30,0, 0, 0),
        }
    };

    public ViewModelY()
    {
        _crossPointD = new LvcPointD();
        PointerMoveCmd = new RelayCommand<PointerCommandArgs>(PointerMove);
    }

    private string ToSuperscript(double value)
    {
        var tt = Math.Pow(10, value);
        var t1 = Math.Floor(Math.Log10(tt)).ToString(CultureInfo.InvariantCulture);
        if (Math.Abs(tt - _crossPointD.Y) <= 0)
            return tt.ToString("0.000");
        //Debug.Print($"To {tt} E{t1}");
        var superscriptDigits = new Dictionary<char, char>
        {
            { '0', '⁰' }, { '1', '¹' }, { '2', '²' }, { '3', '³' },
            { '4', '⁴' }, { '5', '⁵' }, { '6', '⁶' }, { '7', '⁷' },
            { '8', '⁸' }, { '9', '⁹' }, { '-', '⁻' }
        };
        var exponet = "";
        foreach (var c in t1.ToCharArray())
        {
            if (!superscriptDigits.ContainsKey(c)) continue;
            exponet += superscriptDigits[c];
        }
        return $"10{exponet}";
    }
    private void PointerMove(PointerCommandArgs? e)
    {
        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)e?.Chart!;
        var p = chart.ScalePixelsToData(e.PointerPosition);
        _crossPointD = new LvcPointD(p.X, Math.Pow(10, p.Y));
        Debug.Print($"Move {e.PointerPosition.X},{e.PointerPosition.Y} {p.X},{p.Y}");
    }
}
