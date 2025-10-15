# Municipal Services Application - Part 2

**Student Name:** Cherika Bodde  
**Student Number:** st10252644  
**GitHub Repository:** https://github.com/ST10252644/MunicipalAppService.git
**Video Demonstration:** https://youtu.be/IpyiI6u-fs8

---

## Table of Contents
1. Project Overview
2. System Requirements
3. How to Compile and Run
4. How to Use the Application
5. Data Structures Implementation
6. Recommendation Feature
7. Features Implemented
8. Project Structure
9. Troubleshooting
10. Future Enhancements (Part 3)
11. Academic Integrity Statement
12. Contact Information

---

## Project Overview
This is a C# .NET Framework Windows Forms application designed to streamline municipal services in South Africa. The application enables residents to:

- Report issues and request services (Part 1)
- Access information about local events and announcements (Part 2)
- Track service request status (Part 3 - Coming Soon)

**Part 2 Focus:** Local Events and Announcements with advanced data structures and intelligent recommendations.

---

## System Requirements
- **Operating System:** Windows 10 or later
- **Framework:** .NET Framework 4.7.2 or higher
- **IDE:** Visual Studio 2019 or later
- **NuGet Packages:** FontAwesome.Sharp (for icons)

---

## How to Compile and Run

### Method 1: Using Visual Studio
1. **Open the Solution:**
   - Launch Visual Studio
   - Open `MunicipalServicesApp.sln`
2. **Restore NuGet Packages:**
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"
3. **Build the Project:**
   - Press `Ctrl + Shift + B` or select `Build > Build Solution`
4. **Run the Application:**
   - Press `F5` or click the "Start" button (green arrow)

### Method 2: Using Command Line
```bash
cd path/to/MunicipalServicesApp
nuget restore
msbuild MunicipalServicesApp.sln /p:Configuration=Release
cd bin/Release
MunicipalServicesApp.exe
```

---

## How to Use the Application

### 1. Startup Screen
- Launch the application
- Click "Get Started" to access the main menu

### 2. Main Menu (Form1)
- **Report Issues** - Submit community problems (Part 1)
- **Local Events and Announcements** - Browse upcoming events (Part 2)
- **Service Request Status** - Track your requests (Part 3 - Coming Soon)

### 3. Report Issues (Part 1)
- Fill in location, category, and description
- Attach media files if needed
- Click "Submit" to report the issue
- Use "Back to Main Menu" to return

### 4. Local Events and Announcements (Part 2)
**Viewing Events:**
- Events displayed in a table sorted by priority and date
- **Priority Levels:**
  - HIGH (Red) - Urgent
  - MEDIUM (Yellow) - Important
  - LOW (Green) - General

**Searching Events:**
- By category, date, keyword, or combined filters
- Use "Clear Filters" to reset

**Viewing Event Details:**
- Double-click any event row for full details

**Viewing Recommendations:**
- Click "Recommended" button
- Personalized based on search history, viewed events, and high-priority events

**Recent Searches Panel:**
- Shows last 10 searches with timestamps
- Updates automatically

---

## Data Structures Implementation

### 1. Stack (Recently Viewed Events)
```csharp
private Stack<EventInfo> recentlyViewedStack = new Stack<EventInfo>();

// On event view
recentlyViewedStack.Push(ev);
```

### 2. Queue (Search History)
```csharp
private Queue<string> searchHistoryQueue = new Queue<string>();

searchHistoryQueue.Enqueue($"{DateTime.Now:HH:mm} - '{keyword}'");
if (searchHistoryQueue.Count > 10) searchHistoryQueue.Dequeue();
```

### 3. Dictionaries and Sorted Dictionaries
```csharp
private SortedDictionary<DateTime, List<EventInfo>> eventsByDate = new SortedDictionary<DateTime, List<EventInfo>>();
private Dictionary<string, List<EventInfo>> categoryLookup = new Dictionary<string, List<EventInfo>>(StringComparer.OrdinalIgnoreCase);
```

### 4. Sets (Unique Categories and Dates)
```csharp
private HashSet<string> uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
private HashSet<DateTime> eventDates = new HashSet<DateTime>();
```

### 5. Priority Queue (SortedSet for Priority)
```csharp
private SortedSet<EventInfo> priorityEvents = new SortedSet<EventInfo>();

public class EventInfo : IComparable<EventInfo>
{
    public string Title { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; } // 1=HIGH, 2=MEDIUM, 3=LOW

    public int CompareTo(EventInfo other)
    {
        int priorityComparison = this.Priority.CompareTo(other.Priority);
        return priorityComparison != 0 ? priorityComparison : this.Date.CompareTo(other.Date);
    }
}
```

---

## Recommendation Feature
```csharp
private List<EventInfo> GetRecommendedEvents()
{
    var recommendations = new List<EventInfo>();
    // High priority events
    recommendations.AddRange(priorityEvents.Where(e => e.Priority == 1 && e.Date >= DateTime.Today).Take(3));

    // User's most accessed category
    var mostAccessedCategory = categoryAccessCount.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
    if (categoryLookup.ContainsKey(mostAccessedCategory))
        recommendations.AddRange(categoryLookup[mostAccessedCategory].Where(e => !recommendations.Contains(e)).Take(3));

    // Recently viewed category
    if (recentlyViewedStack.Any())
    {
        var recentCategory = recentlyViewedStack.Peek().Category;
        recommendations.AddRange(categoryLookup[recentCategory].Where(e => !recommendations.Contains(e)).Take(2));
    }

    // Fill with upcoming events
    recommendations.AddRange(eventsByDate.SelectMany(kv => kv.Value).Where(e => !recommendations.Contains(e)).Take(8 - recommendations.Count));
    return recommendations;
}
```

---

## Features Implemented

### Part 1
- Startup screen, main menu
- Report Issues with location, category, description, media, progress bar, validation

### Part 2
- **Local Events Table:**
```csharp
// Display events with color coding based on priority
foreach (var ev in events)
{
    dgvEvents.Rows.Add(ev.Title, ev.Category, ev.Date.ToShortDateString(), ev.Priority);
    dgvEvents.Rows[dgvEvents.Rows.Count - 1].DefaultCellStyle.BackColor = ev.Priority == 1 ? Color.Red : ev.Priority == 2 ? Color.Yellow : Color.Green;
}
```

- **Advanced Filtering:**
```csharp
var results = events;
if (selectedCategory != null) results = results.Where(ev => ev.Category == selectedCategory).ToList();
if (dateFilter.HasValue) results = results.Where(ev => ev.Date.Date == dateFilter.Value).ToList();
if (!string.IsNullOrEmpty(keyword)) results = results.Where(ev => ev.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
```

- **Search History Tracking:**
```csharp
searchHistoryQueue.Enqueue($"{DateTime.Now:HH:mm} - '{keyword}'");
```

- **Event Details View:**
```csharp
private void dgvEvents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
{
    var selectedEvent = events[e.RowIndex];
    MessageBox.Show($"{selectedEvent.Title}\n{selectedEvent.Category}\n{selectedEvent.Date}\n{selectedEvent.Description}");
}
```

- **Intelligent Recommendations:** Included in `GetRecommendedEvents()` code snippet above
- **Data Structures:** Stack, Queue, Dictionary, SortedDictionary, HashSet, SortedSet (see above)
- **Modern UI:** Panels, hover effects, color-coded priority badges

### Part 3 (Coming Soon)
- Service Request Status tracking
- Tree, graph, AVL, and heap data structures
- Route optimization using MST

---

## Project Structure
```
MunicipalServicesApp/
│
├── Forms/
│   ├── StartupForm.cs
│   ├── Form1.cs
│   ├── ReportIssuesForm.cs
│   ├── LocalEventsForm.cs
│   └── RecommendedEventsForm.cs
│

├── img/
│   └── (Icons and images)
│
└── README.md
```

---

## Troubleshooting
- **Application won't start:** Install .NET Framework 4.7.2+
- **Missing icons:** Restore NuGet packages
- **Events not displaying:** Ensure sample data is loaded
- **Recommendations not updating:** Interact with events to build user history

---

## Future Enhancements (Part 3)
- Service Request Status tracking with unique IDs
- BST for request lookups
- Graph structures for dependencies
- AVL Tree for balanced storage
- Heap for priority handling
- MST for route optimization

---

## Academic Integrity Statement
This project was completed individually by Cherika Bodde (st10252644) for PROG7312. All code is original unless cited. FontAwesome.Sharp library is used under its license.

---

## References
- C# Corner, 2023. Understanding Data Structures in C#. Available at: https://www.c-sharpcorner.com/article/data-structures-in-c-sharp/ [Accessed 10 October 2025].
- Department of Cooperative Governance and Traditional Affairs, 2023. Improving Municipal Service Delivery. Republic of South Africa. Available at: https://www.cogta.gov.za/ [Accessed 8 October 2025].
- DotNetPerls, 2024. C# SortedSet Examples. Available at: https://www.dotnetperls.com/sortedset [Accessed 11 October 2025].
- FontAwesome, 2024. FontAwesome.Sharp Documentation. Available at: https://github.com/awesome-inc/FontAwesome.Sharp [Accessed 13 October 2025].
- GeeksforGeeks, 2024. Data Structures Tutorial. Available at: https://www.geeksforgeeks.org/data-structures/ [Accessed 9 October 2025].
- Jamro, M., 2024. C# Data Structures and Algorithms - Second Edition.
- Material Design, 2024. Design Guidelines. Google. Available at: https://material.io/design [Accessed 12 October 2025].
- Microsoft, 2024. Dictionary<TKey,TValue> Class (System.Collections.Generic). Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 [Accessed 13 October 2025].
- Microsoft, 2024. Queue<T> Class (System.Collections.Generic). Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1 [Accessed 13 October 2025].
- Microsoft, 2024. SortedDictionary<TKey,TValue> Class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2 [Accessed 13 October 2025].
- Microsoft, 2024. Stack<T> Class (System.Collections.Generic). Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1 [Accessed 13 October 2025].
- Microsoft, 2024. Windows Forms Overview. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview/ [Accessed 12 October 2025].
- Ricci, F., Rokach, L. and Shapira, B., 2022. Recommender Systems Handbook. 3rd ed. New York: Springer. DOI: 10.1007/978-1-0716-2197-4
- Stack Overflow, 2024. When to use Stack vs Queue in C#?. Available at: https://stackoverflow.com/questions/3825050/when-to-use-stack-vs-queue [Accessed 11 October 2025].
- Tutorialspoint, 2024. C# - Collections. Available at: https://www.tutorialspoint.com/csharp/csharp_collections.htm [Accessed 9 October 2025].
- Towards Data Science, 2023. Building a Simple Recommendation System. Medium. Available at: https://towardsdatascience.com/building-a-recommendation-system-e6cb0c1e5673 [Accessed 9 October 2025].


