//  Принципы SOLID

//  Термин "SOLID" представляет собой акроним для набора практик проектирования программного кода и построения гибкой и
//  адаптивной программы. Данный термин был введен известным американским специалистом в области программирования
//  Робертом Мартином (Robert Martin), более известным как "дядюшка Боб" или Uncle Bob.

//  Сам акроним образован по первым буквам названий SOLID-принципов:
//  Single Responsibility Principle (Принцип единственной обязанности)
//  Open / Closed Principle(Принцип открытости / закрытости)
//  Liskov Substitution Principle (Принцип подстановки Лисков)
//  Interface Segregation Principle (Принцип разделения интерфейсов)
//  Dependency Inversion Principle (Принцип инверсии зависимостей)

//  Принципы SOLID - это не паттерны, их нельзя назвать какими-то определенными догмами, которые надо обязательно применять
//  при разработке, однако их использование позволит улучшить код программы, упростить возможные его изменения и поддержку.


#region Принцип единственной обязанности
//  Принцип единственной обязанности (Single Responsibility Principle) можно сформулировать так:
//  Каждый компонент должен иметь одну и только одну причину для изменения.

//  В C# в качестве компонента может выступать класс, структура, метод. А под обязанностью здесь понимается набор действий,
//  которые выполняют единую задачу. То есть суть принципа заключается в том, что класс/структура/метод должны выполнять одну
//  единственную задачу. Весь функционал компонента должен быть целостным, обладать высокой связностью (high cohesion).

//  Конкретное применение принципа зависит от контекста. В данном случае важно понимать, как изменяется компонент. Если он
//  выполняет несколько различных действий, и они изменяются по отдельности, то это как раз тот случай, когда можно применить
//  принцип единственной обязанности. То есть иными словами, у компонента несколько причин для изменения.

//  Допустим, нам надо определить класс отчета, по которому мы можем перемещаться по страницам и который можно выводить на печать.
//  На первый взгляд мы могли бы определить следующий класс:

class Report
{
    public string Text { get; set; } = "";
    public void GoToFirstPage() =>
        Console.WriteLine("Переход к первой странице");

    public void GoToLastPage() =>
        Console.WriteLine("Переход к последней странице");

    public void GoToPage(int pageNumber) =>
        Console.WriteLine($"Переход к странице {pageNumber}");


    public void Print()
    {
        Console.WriteLine("Печать отчета");
        Console.WriteLine(Text);
    }
}

//  Ключевым понятием применительно к данному принципу является cohesion или связность/согласованность. Это понятие описывает,
//  насколько близко связаны компоненты. Чем больше связность между компонентами, тем больше программа соответствует принципу единой
//  ответственности

//  Например, первые три метода класса относятся к навигации по отчету и представляют одно единое функциональное целое, обладают
//  высокой связностью. От них отличается метод Print, который производит печать. Что если нам понадобится печатать отчет на консоль
//  или передать его на принтер для физической печати на бумаге? Или вывести в файл? Сохранить в формате html, txt, rtf и т.д.?
//  Очевидно, что мы можем для этого поменять нужным образом метод Print(). Однако это вряд ли затронет остальные методы, которые
//  относятся к навигации страницы.

//  Также верно и обратное - изменение методов постраничной навигации вряд ли повлияет на возможность вывода текста отчета на
//  принтер или на консоль. Таким образом, у нас здесь прослеживаются две причины для изменения, значит, класс Report обладает
//  двумя обязанностями, и от одной из них этот класс надо освободить. Решением было бы вынести каждую обязанность в отдельный
//  компонент (в данном случае в отдельный класс):

class Report2
{
    public string Text { get; set; } = "";
    public void GoToFirstPage() =>
        Console.WriteLine("Переход к первой странице");

    public void GoToLastPage() =>
        Console.WriteLine("Переход к последней странице");

    public void GoToPage(int pageNumber) =>
        Console.WriteLine($"Переход к странице {pageNumber}");
}
//  обязанность - печать отчета
class Printer
{
    public void PrintReport(Report2 report)
    {
        Console.WriteLine("Печать отчета");
        Console.WriteLine(report.Text);
    }
}

//  Теперь печать вынесена в отдельный класс Printer, который через метод Print получает объект отчета и выводит его текст на консоль.
#endregion

#region Второй пример
//  Стоит понимать, что обязанности в классах не всегда группируются по методам. Речь идет именно об обязанности компонента, в качестве
//  которого может выступать не только тип (например, класс), но и метод или свойство. И вполне возможно, что в одном каком-то методе
//  сгруппировано несколько обязанностей. Например:

class Phone1
{
    public string Model { get;}
    public int Price { get;}
    public Phone1(string model, int price)
    {
        Model = model;
        Price = price;
    }
}
 
class MobileStore1
{
    List<Phone1> phones1 = new();
    public void Process()
    {
        // ввод данных
        Console.WriteLine("Введите модель:");
        string? model = Console.ReadLine();
        Console.WriteLine("Введите цену:");
 
        // валидация
        bool result = int.TryParse(Console.ReadLine(), out var price);
 
        if (result == false || price <= 0 || string.IsNullOrEmpty(model))
        {
            throw new Exception("Некорректно введены данные");
        }
        else
        {
            phones1.Add(new Phone1(model, price));
            // сохраняем данные в файл
            using (StreamWriter writer = new StreamWriter("store.txt", true))
            {
                writer.WriteLine(model);
                writer.WriteLine(price);
            }
            Console.WriteLine("Данные успешно обработаны");
        }
    }
}

//Класс имеет один единственный метод Process, однако этот небольшой метод, содержит в себе как минимум четыре обязанности:
//ввод данных, их валидация, создание объекта Phone и сохранение. В итоге класс знает абсолютно все: как получать данные, как
//валидировать, как сохранять. При необходимости в него можно было бы засунуть еще пару обязанностей. Такие классы еще называют
//"божественными" или "классы-боги", так как они инкапсулируют в себе абсолютно всю функциональность. Подобные классы являются
//одним из распространенных анти-паттернов, и их применения надо стараться избегать.

//Хотя тут довольно немного кода, однако при последующих изменениях метод Process может быть сильно раздут, а функционал усложнен и запутан.

//Теперь изменим код класса, инкапсулировав все обязанности в отдельных классах:

class Phone
{
    public string Model { get; }
    public int Price { get; }
    public Phone(string model, int price)
    {
        Model = model;
        Price = price;
    }
}

class MobileStore
{
    List<Phone> phones = new List<Phone>();

    public IPhoneReader Reader { get; set; }
    public IPhoneBinder Binder { get; set; }
    public IPhoneValidator Validator { get; set; }
    public IPhoneSaver Saver { get; set; }

    public MobileStore(IPhoneReader reader, IPhoneBinder binder, IPhoneValidator validator, IPhoneSaver saver)
    {
        this.Reader = reader;
        this.Binder = binder;
        this.Validator = validator;
        this.Saver = saver;
    }

    public void Process()
    {
        string?[] data = Reader.GetInputData();
        Phone phone = Binder.CreatePhone(data);
        if (Validator.IsValid(phone))
        {
            phones.Add(phone);
            Saver.Save(phone, "store.txt");
            Console.WriteLine("Данные успешно обработаны");
        }
        else
        {
            Console.WriteLine("Некорректные данные");
        }
    }
}

interface IPhoneReader
{
    string?[] GetInputData();
}
class ConsolePhoneReader : IPhoneReader
{
    public string?[] GetInputData()
    {
        Console.WriteLine("Введите модель:");
        string? model = Console.ReadLine();
        Console.WriteLine("Введите цену:");
        string? price = Console.ReadLine();
        return new string?[] { model, price };
    }
}

interface IPhoneBinder
{
    Phone CreatePhone(string?[] data);
}

class GeneralPhoneBinder : IPhoneBinder
{
    public Phone CreatePhone(string?[] data)
    {
        if (data is { Length: 2 } && data[0] is string model &&
            model.Length > 0 && int.TryParse(data[1], out var price))
        {
            return new Phone(model, price);

        }
        throw new Exception("Ошибка привязчика модели Phone. Некорректные данные");
    }
}

interface IPhoneValidator
{
    bool IsValid(Phone phone);
}

class GeneralPhoneValidator : IPhoneValidator
{
    public bool IsValid(Phone phone) =>
        !string.IsNullOrEmpty(phone.Model) && phone.Price > 0;
}

interface IPhoneSaver
{
    void Save(Phone phone, string fileName);
}

class TextPhoneSaver : IPhoneSaver
{
    public void Save(Phone phone, string fileName)
    {
        using StreamWriter writer = new StreamWriter(fileName, true);
        writer.WriteLine(phone.Model);
        writer.WriteLine(phone.Price);
    }
}

//  Возможное применение класса:

//MobileStore store = new MobileStore(
//    new ConsolePhoneReader(), new GeneralPhoneBinder(),
//    new GeneralPhoneValidator(), new TextPhoneSaver());
//store.Process();

//  Теперь для каждой обязанности определен свой интерфейс. Конкретные реализации обязанностей устнавливаются в виде интрефейсов в
//  целевом классе.

//  В то же время кода стало больше, в связи с чем программа усложнилась. И, возможно, подобное усложнение может показаться
//  неоправданным при наличии одного небольшого метода, который необязательно будет изменяться. Однако при модификации стало гораздо
//  проще вводить новый функционал без изменения существующего кода. А все части метода Process, будучи инкапсулированными во внешних
//  классах, теперь не зависят друг от друга и могут изменяться самостоятельно.
#endregion

#region Распространенные случаи отхода от принципа SRP
//  Нередко принцип единственной обязанности нарушает при смешивании в одном классе функциональности разных уровней. Например, класс
//  производит вычисления и выводит их пользователю, то есть соединяет в себя бизнес-логику и работу с пользовательским интерфейсом.
//  Либо класс управляет сохранением/получением данных и выполнением над ними вычислений, что также нежелательно. Класс следует
//  применять только для одной задачи - либо бизнес-логика, либо вычисления, либо работа с данными.

//  Другой распространенный случай - наличие в классе или его методах абсолютно несвязанного между собой функционала.
#endregion

#region Распространенные сценарии выделения компонентов
//  Есть ряд распространенных сценариев, которые обычно выносятся в отдельные компоненты:
//      Логика хранения данных
//      Валидация
//      Механизм уведомлений пользователя
//      Обработка ошибок
//      Логгирование
//      Выбор класса или создание его объекта
//      Форматирование
//      Парсинг
//      Маппинг данных
#endregion
