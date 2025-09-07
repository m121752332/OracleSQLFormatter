# Oracle SQL Formatter 規格文件

## 1. 專案概述
- **專案名稱**: OracleSQL.Formatter
- **開發框架**: .NET 8 + Avalonia UI
- **支援平台**: Windows, Linux, macOS
- **主要功能**: Oracle SQL 語法美化與格式化

## 2. 專案結構

```
OracleSQL.Formatter/
├── src/
│   ├── OracleSQL.Formatter/                 # 主應用程式
│   │   ├── App.axaml
│   │   ├── App.axaml.cs
│   │   ├── Program.cs
│   │   ├── ViewLocator.cs
│   │   └── OracleSQL.Formatter.csproj
│   │
│   ├── OracleSQL.Formatter.Core/            # 核心業務邏輯
│   │   ├── Formatters/
│   │   │   ├── ISqlFormatter.cs
│   │   │   └── OracleSqlFormatter.cs
│   │   ├── Models/
│   │   │   ├── FormatterOptions.cs
│   │   │   └── FormatterResult.cs
│   │   ├── Services/
│   │   │   ├── IFormatterService.cs
│   │   │   └── FormatterService.cs
│   │   └── OracleSQL.Formatter.Core.csproj
│   │
│   ├── OracleSQL.Formatter.UI/              # UI 元件
│   │   ├── Views/
│   │   │   ├── MainWindow.axaml
│   │   │   ├── MainWindow.axaml.cs
│   │   │   ├── FormatterView.axaml
│   │   │   ├── FormatterView.axaml.cs
│   │   │   ├── SettingsView.axaml
│   │   │   ├── SettingsView.axaml.cs
│   │   │   ├── AboutView.axaml
│   │   │   └── AboutView.axaml.cs
│   │   ├── ViewModels/
│   │   │   ├── ViewModelBase.cs
│   │   │   ├── MainWindowViewModel.cs
│   │   │   ├── FormatterViewModel.cs
│   │   │   ├── SettingsViewModel.cs
│   │   │   └── AboutViewModel.cs
│   │   ├── Controls/
│   │   │   └── SidebarControl.axaml
│   │   ├── Converters/
│   │   │   └── BoolToVisibilityConverter.cs
│   │   ├── Resources/
│   │   │   ├── Strings.resx
│   │   │   ├── Strings.zh-TW.resx
│   │   │   ├── Strings.zh-CN.resx
│   │   │   └── Strings.en-US.resx
│   │   └── OracleSQL.Formatter.UI.csproj
│   │
│   └── OracleSQL.Formatter.Common/          # 共用元件
│       ├── Enums/
│       │   ├── CaseOption.cs
│       │   └── Language.cs
│       ├── Extensions/
│       │   └── StringExtensions.cs
│       ├── Helpers/
│       │   └── LocalizationHelper.cs
│       └── OracleSQL.Formatter.Common.csproj
│
├── tests/
│   ├── OracleSQL.Formatter.Core.Tests/
│   │   ├── Formatters/
│   │   │   └── OracleSqlFormatterTests.cs
│   │   ├── Services/
│   │   │   └── FormatterServiceTests.cs
│   │   └── OracleSQL.Formatter.Core.Tests.csproj
│   │
│   └── OracleSQL.Formatter.UI.Tests/
│       ├── ViewModels/
│       │   └── FormatterViewModelTests.cs
│       └── OracleSQL.Formatter.UI.Tests.csproj
│
├── docs/
│   ├── README.md
│   ├── SPEC.md                              # 本規格文件
│   ├── USER_GUIDE.md
│   └── API_DOCUMENTATION.md
│
├── .gitignore
├── OracleSQLFormatter.sln
└── README.md
```

## 3. 功能規格

### 3.1 主要功能模組

#### 3.1.1 美化器 (Formatter)
- **輸入區域**: 多行文字編輯器，支援貼上 SQL
- **輸出區域**: 顯示美化後的 SQL
- **美化按鈕**: 執行美化操作
- **複製按鈕**: 複製美化後的結果
- **清空按鈕**: 清空輸入內容

#### 3.1.2 設置 (Settings)
- **多語言設定**: 繁體中文、簡體中文、英文
- **格式化選項**:
  - 關鍵字大小寫: Uppercase | Lowercase | InitCap
  - 表名大小寫: Uppercase | Lowercase | Unchanged
  - 欄位名大小寫: Uppercase | Lowercase | Unchanged
  - 函數名大小寫: InitCap | Uppercase | Lowercase
  - 資料型別大小寫: Uppercase | Lowercase
  - 變數大小寫: Unchanged | Uppercase | Lowercase
  - 別名大小寫: Unchanged | Uppercase | Lowercase
  - 引號識別符大小寫: Unchanged | Uppercase | Lowercase
  - 其他識別符大小寫: Lowercase | Uppercase | Unchanged
  
- **換行與縮排**:
  - 每行最大長度: 預設 80，範圍 40-200
  - 換行後使用逗號: After | Before | Before with space
  - 列表和參數樣式: Stacked | Not Stacked
  - 堆疊對齊: Align left | Align right
  - AND/OR under WHERE 子句: 是/否
  - 移除美化前的換行: 是/否
  - 修剪每行引號字元: 是/否
  - 緊湊模式: 是/否
  - 緊湊模式下每行最大長度: 預設 80

#### 3.1.3 關於 (About)
- 應用程式版本資訊
- 開發者資訊
- 授權資訊

### 3.2 UI/UX 設計

#### 3.2.1 整體佈局
- **側邊欄**: 固定寬度 200px，包含導航選單
- **主內容區**: 自適應寬度，RWD 響應式設計
- **主題**: 現代化深色/淺色主題切換

#### 3.2.2 設計原則
- Material Design 風格
- 圓角設計元素
- 陰影效果
- 平滑過渡動畫
- 響應式佈局

## 4. 技術規格

### 4.1 核心技術
- **.NET SDK**: 8.0
- **UI Framework**: Avalonia 11.0+
- **MVVM Framework**: ReactiveUI
- **單元測試**: xUnit
- **程式語言**: C# 12

### 4.2 NuGet 套件
```xml
<PackageReference Include="Avalonia" Version="11.3.5" />
<PackageReference Include="Avalonia.Desktop" Version="11.3.5" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.3.5" />
<PackageReference Include="Avalonia.ReactiveUI" Version="11.3.5" />
<PackageReference Include="ReactiveUI" Version="19.5.31" />
<PackageReference Include="xunit" Version="2.6.1" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
```

## 5. SQL 美化規則

### 5.1 基本規則
1. **關鍵字處理**: SELECT, FROM, WHERE, ORDER BY 等關鍵字依設定調整大小寫
2. **縮排**: 使用 4 個空格
3. **換行**: 
   - 主要子句（SELECT, FROM, WHERE 等）獨立一行
   - 超過最大長度自動換行
4. **對齊**: 
   - 列表項目垂直對齊
   - AND/OR 條件對齊

### 5.2 範例

**輸入**:
```sql
select emp.employee_id,emp.first_name,emp.last_name,dept.department_name from employees emp inner join departments dept on emp.department_id=dept.department_id where emp.salary>5000 and dept.location_id=1700 order by emp.last_name,emp.first_name
```

**輸出**:
```sql
SELECT emp.employee_id,
       emp.first_name,
       emp.last_name,
       dept.department_name
FROM employees emp
INNER JOIN departments dept
    ON emp.department_id = dept.department_id
WHERE emp.salary > 5000
    AND dept.location_id = 1700
ORDER BY emp.last_name,
         emp.first_name
```

## 6. 測試規格

### 6.1 單元測試案例
1. **格式化器測試**
   - 關鍵字大小寫轉換
   - 換行處理
   - 縮排處理
   - 特殊字元處理

2. **服務測試**
   - 配置載入
   - 多語言切換
   - 設定儲存與讀取

3. **ViewModel 測試**
   - 命令執行
   - 資料綁定
   - 狀態管理

## 7. 部署需求

### 7.1 系統需求
- **Windows**: Windows 10 或更高版本
- **Linux**: Ubuntu 20.04 或更高版本
- **macOS**: macOS 10.15 或更高版本
- **.NET Runtime**: 8.0 或更高版本

### 7.2 建置指令
```bash
# 建置所有平台
dotnet build

# 發佈 Windows
dotnet publish -c Release -r win-x64 --self-contained

# 發佈 Linux
dotnet publish -c Release -r linux-x64 --self-contained

# 發佈 macOS
dotnet publish -c Release -r osx-x64 --self-contained
```

## 8. 版本控制

### 8.1 Git 分支策略
- **main**: 穩定版本
- **develop**: 開發版本
- **feature/**: 功能開發
- **bugfix/**: 錯誤修復
- **release/**: 發布準備

### 8.2 版本號規則
- 遵循語意化版本 2.0.0
- 格式: MAJOR.MINOR.PATCH

## 9. 未來擴展

### 9.1 計劃功能
- 支援更多 SQL 方言（MySQL, PostgreSQL, SQL Server）
- SQL 語法高亮
- SQL 語法檢查
- 批次處理多個 SQL 檔案
- 匯出設定檔
- 外掛系統

### 9.2 效能優化
- 大型 SQL 檔案處理優化
- 非同步處理
- 快取機制

## 10. 授權
MIT License
