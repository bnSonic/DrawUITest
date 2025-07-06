using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DrawUITest.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public BaseViewModel()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    public virtual void OnPropertyChangedAll() => OnPropertyChanged(new PropertyChangedEventArgs(""));

    protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
        string propertyName = GetPropertyName(propertyExpression);
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        var eventHandler = PropertyChanged;
        eventHandler?.Invoke(this, args);
    }

    protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
    {
        try
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            this.OnPropertyChanged(propertyName);
        }
        catch 
        {
            return false;
        }

        return true;
    }
    
    private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        if (propertyExpression == null)
            throw new ArgumentNullException(nameof(propertyExpression));

        if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
            throw new ArgumentException("Should be a member access lambda expression", nameof(propertyExpression));

        var memberExpression = (MemberExpression)propertyExpression.Body;

        return memberExpression.Member.Name;
    }
}