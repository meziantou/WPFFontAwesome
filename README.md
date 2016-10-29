Meziantou.WpfFontAwesome
=======

Use FontAwesome in WPF application

# Usage

1. Install Nuget package
2. Add the `ResourceDictionary` to the `app.xaml` file

````xaml
<Application x:Class="Meziantou.WpfFontAwesome.Demo.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Meziantou.WpfFontAwesome;component/FontAwesome.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
````

3. Add the xml namespace `xmlns:fa="clr-namespace:Meziantou.WpfFontAwesome;assembly=Meziantou.WpfFontAwesome"`
4. Use it

````xaml
<TextBlock Text="{fa:Icon Beer}" Style="{StaticResource FontAwesome}" />

<!-- Animation -->
<TextBlock Text="{fa:Icon Spinner}" Style="{StaticResource FontAwesomeSpin}" />
<TextBlock Text="{fa:Icon Spinner}" Style="{StaticResource FontAwesomePulse}" />
````