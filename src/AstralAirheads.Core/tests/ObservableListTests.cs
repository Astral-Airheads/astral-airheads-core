using System;
using System.IO;
using AstralAirheads.Collections.Generic;

namespace AstralAirheads;

public sealed class ObservableListTests
{
    private readonly ObservableList<TextWriter> _observable = [];

    #region Event Tests
    [Fact]
    public void ObservableList_DoesInvokeAddEvent()
    {
        var doesInvoke = false;
        _observable.ItemAdded += (sender, e) =>
        {
            doesInvoke = true;
        };

        _observable.Add(Console.Out);

        if (!doesInvoke)
            Assert.Fail("ItemAdded event wasn't invoked!");
    }

    [Fact]
    public void ObservableList_DoesInvokeRemovedEvent()
    {
        var doesInvokeAdd = false;
        _observable.ItemAdded += (sender, e) =>
        {
            doesInvokeAdd = true;
        };

        _observable.Add(Console.Out);

        if (!doesInvokeAdd)
            Assert.Fail("ItemAdded event wasn't invoked!");

        var doesInvokeRemoved = false;
        _observable.ItemRemoved += (sender, e) =>
        {
            doesInvokeRemoved = true;
        };

        _observable.Remove(Console.Out);

        if (!doesInvokeRemoved)
            Assert.Fail("ItemRemoved event wasn't invoked!");
    }
    #endregion
}
