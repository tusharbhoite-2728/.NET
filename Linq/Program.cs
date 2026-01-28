using Linq;


IEnumerable<Student> students =
[
    new Student(First: "Svetlana", Last: "Omelchenko", ID: 111, Scores: [97, 92, 81, 60]),
    new Student(First: "Claire",   Last: "O'Donnell",  ID: 112, Scores: [75, 84, 91, 39]),
    new Student(First: "Sven",     Last: "Mortensen",  ID: 113, Scores: [88, 94, 65, 91]),
    new Student(First: "Cesar",    Last: "Garcia",     ID: 114, Scores: [97, 89, 85, 82]),
    new Student(First: "Debra",    Last: "Garcia",     ID: 115, Scores: [35, 72, 91, 70]),
    new Student(First: "Fadi",     Last: "Fakhouri",   ID: 116, Scores: [99, 86, 90, 94]),
    new Student(First: "Hanying",  Last: "Feng",       ID: 117, Scores: [93, 92, 80, 87]),
    new Student(First: "Hugo",     Last: "Garcia",     ID: 118, Scores: [92, 90, 83, 78]),

    new Student("Lance",   "Tucker",      119, [68, 79, 88, 92]),
    new Student("Terry",   "Adams",       120, [99, 82, 81, 79]),
    new Student("Eugene",  "Zabokritski", 121, [96, 85, 91, 60]),
    new Student("Michael", "Tucker",      122, [94, 92, 91, 91])
];
Console.WriteLine(System.Environment.NewLine);

Console.WriteLine("================================================================================");
Console.WriteLine("================================================================================");

IEnumerable<Student> studentQuery =
    from student in students
    where student.Scores[0] > 90 && student.Scores[3] < 80
    select student;



foreach (Student student in studentQuery)
{
    Console.WriteLine($"{student.Last}, {student.First}");
}


Console.WriteLine();

Console.WriteLine("================================================================================");
Console.WriteLine("================================================================================");


int[] scores = [45, 85, 24, 90, 49, 98];

IEnumerable<int> Query =
    from score  in scores
    orderby score descending
    select score;


foreach (int score in Query)
{
    Console.WriteLine(score);
}


Console.WriteLine();

Console.WriteLine("================================================================================");
Console.WriteLine("================================================================================");



int[] numbers = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,35,63,94,27,56];

//Query syntax:
IEnumerable<int> numQuery1 =
    from num in numbers
    where num % 2 == 0
    orderby num
    select num;

//Method syntax:
IEnumerable<int> numQuery2 = numbers
    .Where(num => num % 2 == 0)
    .OrderBy(n => n);

foreach (int i in numQuery1)
{
    Console.Write(i + " ");
}
Console.WriteLine(System.Environment.NewLine);
foreach (int i in numQuery2)
{
    Console.Write(i + " ");
}

Console.WriteLine();
