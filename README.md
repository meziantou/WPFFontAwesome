Meziantou.WpfFontAwesome
=======

Use FontAwesome 5.6.3 in WPF application

# Usage

1. Install Nuget package `Meziantou.WpfFontAwesome`
2. Add the `ResourceDictionary` to the `app.xaml` file

````xaml
<Application x:Class="Meziantou.WpfFontAwesome.Demo.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Meziantou.WpfFontAwesome;component/Themes/Generic.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
````

3. Add the xml namespace `xmlns:fa="clr-namespace:Meziantou.WpfFontAwesome;assembly=Meziantou.WpfFontAwesome"`
4. Use it

````xaml
<fa:FontAwesomeIcon SolidIcon="AddressBook" FontSize="60" />
<fa:FontAwesomeIcon RegularIcon="AddressBook" FontSize="60" />
<fa:FontAwesomeIcon BrandIcon="Microsoft" FontSize="60" />
<fa:FontAwesomeIcon SolidIcon="Spinner" AnimationType="Spin" FontSize="60" />
<fa:FontAwesomeIcon SolidIcon="Spinner" AnimationType="Pulse" FontSize="60" />

<TextBlock Text="{fa:Icon AddressBook}" Style="{StaticResource FontAwesomeRegular}" FontSize="60" />
<TextBlock Text="{fa:Icon AddressBook}" Style="{StaticResource FontAwesomeSolid}" FontSize="60" />
<TextBlock Text="{fa:Icon FontAwesome}" Style="{StaticResource FontAwesomeBrand}" FontSize="60" />
<TextBlock Text="{fa:Icon Spinner}" Style="{StaticResource FontAwesomeSolidSpin}" FontSize="60" />
<TextBlock Text="{fa:Icon Spinner}" Style="{StaticResource FontAwesomeSolidPulse}" FontSize="60" />
````

If you want to use a Pro icon, you need to load the font first:

```csharp
FontAwesomeIcon.ProSolidFontFamily = new FontFamily(...);
```
