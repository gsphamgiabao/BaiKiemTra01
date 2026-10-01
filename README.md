# BaiKiemTra01
Phạm Gia Bảo - 24810320309 - D19QTANM1

I. PHẦN LÝ THUYẾT & CÂU HỎI NGẮN

Câu 1: Khác nhau giữa Value Types và Reference Types về cơ chế lưu trữ vùng nhớ (Stack vs Heap)

Kiểu giá trị (Value Types):

Các kiểu đại diện: int, float, double, bool, char, struct, enum, Nullable.

Vùng nhớ lưu trữ: Lưu trữ trực tiếp giá trị của biến trên vùng nhớ Stack (khi là biến cục bộ trong hàm) hoặc thuộc về vùng nhớ chứa nó (nếu là field của một class nằm trên Heap).

Hành vi sao chép (Pass-by-value): Khi gán hoặc truyền tham số, C# tạo ra một bản sao độc lập hoàn toàn. Thay đổi giá trị trên bản sao không ảnh hưởng đến biến gốc.

Quản lý bộ nhớ: Tự động giải phóng ngay khi vượt ra khỏi phạm vi (scope) của hàm hoặc khối lệnh.

Giá trị mặc định / Null: Không thể gán giá trị null (trừ trường hợp dùng Nullable hoặc T?).

Kiểu tham chiếu (Reference Types):

Các kiểu đại diện: class, interface, delegate, string, object, array.

Vùng nhớ lưu trữ: Biến con trỏ/tham chiếu (chứa địa chỉ bộ nhớ) được lưu trên Stack, trong khi đối tượng dữ liệu thực tế được cấp phát trên Heap.

Hành vi sao chép (Pass-by-reference): Khi gán hoặc truyền tham số, C# chỉ sao chép địa chỉ tham chiếu. Cả hai biến sẽ cùng trỏ tới một vùng nhớ trên Heap. Thay đổi dữ liệu qua một biến sẽ làm thay đổi dữ liệu của biến còn lại.

Quản lý bộ nhớ: Được quản lý và thu gom tự động bởi tiến trình Garbage Collector (GC) trên Heap.

Giá trị mặc định / Null: Có thể nhận giá trị null (trạng thái chưa trỏ đến đối tượng nào trên Heap).


Câu 2: Init-only Properties (init) trong C# 9/10 và trường hợp sử dụng thực tế

Sự khác nhau giữa "init" và "set" thông thường:

Thuộc tính dùng "set": Cho phép gán hoặc thay đổi giá trị của thuộc tính bất kỳ lúc nào trong suốt vòng đời hoạt động của đối tượng.

Thuộc tính dùng "init" (Init-only Property): Chỉ cho phép gán giá trị duy nhất một lần trong quá trình khởi tạo đối tượng (thông qua Constructor hoặc Object Initializer). Sau khi đối tượng hoàn tất khởi tạo, thuộc tính này trở thành Read-only (chỉ đọc) và không thể chỉnh sửa thêm.

Trường hợp sử dụng thực tế:

Tạo các đối tượng bất biến (Immutable Objects): Giúp đảm bảo trạng thái đối tượng không bị thay đổi ngoài ý muốn sau khi tạo, tránh các lỗi phụ (side-effects) trong lập trình đa luồng hoặc xử lý dữ liệu phức tạp.

Cú pháp khởi tạo linh hoạt: Cho phép người dùng sử dụng cú pháp Object Initializer { Property = value } ngắn gọn thay vì phải viết các hàm khởi tạo (Constructors) quá nhiều tham số.

Ứng dụng cụ thể: Định nghĩa các lớp DTO (Data Transfer Objects), các Model chứa thông tin cấu hình (Configuration Settings), Model Request/Response trong API, hoặc các thuộc tính định danh cố định không đổi (như Id, CreatedDate).

Ví dụ đoạn mã C#:
public class Person
{
public int Id { get; init; } // Chỉ gán khi khởi tạo
public string Name { get; set; } // Có thể sửa bất kỳ lúc nào
}

Cách sử dụng:
var p = new Person { Id = 1, Name = "Alice" }; // Hợp lệ
p.Name = "Bob"; // Hợp lệ
// p.Id = 2; // BÁO LỖI BIÊN DỊCH: Auto-property 'Id' cannot be assigned to -- it is read only

Câu 3: Phân biệt phương thức virtual ở lớp cha và phương thức override ở lớp con

Khi triển khai tính Đa hình (Polymorphism):

Phương thức virtual (ở Lớp cha):

Định nghĩa hành vi mặc định của phương thức tại lớp cơ sở.

Đóng vai trò là "điểm mở" (extension point), cho phép các lớp con có quyền ghi đè (override) để thay đổi hành vi nếu cần.

Nếu lớp con không ghi đè, nó sẽ tự động tái sử dụng logic mặc định của lớp cha.

Phương thức override (ở Lớp con):

Được khai báo ở lớp dẫn xuất để ghi đè và thay thế hoàn toàn logic xử lý mặc định của phương thức virtual từ lớp cha.

Thực thi cơ chế Liên kết muộn (Late Binding / Dynamic Binding): Khi gọi phương thức thông qua một biến khai báo kiểu lớp cha nhưng tham chiếu tới thể hiện của lớp con, C# sẽ tự động kích hoạt phương thức override của lớp con tại thời điểm thực thi (Runtime).

Ví dụ đoạn mã C#:
public class Animal
{
public virtual void Speak() => Console.WriteLine("Animal makes a sound");
}

public class Dog : Animal
{
public override void Speak() => Console.WriteLine("Dog barks");
}

Thực thi:
Animal myPet = new Dog();
myPet.Speak(); // Kết quả in ra: "Dog barks" (Gọi phương thức override của Dog)

Câu 4: Lý do thành phần static không thể truy xuất thông qua thể hiện (Object Instance)

Trong C#, một thành phần khai báo static không thể truy xuất qua đối tượng tạo bởi toán tử new vì các lý do sau:

Bản chất thuộc về Lớp (Class-level) thay vì Thể hiện (Instance-level):

Thành phần static thuộc về bản thân Lớp và được cấp phát duy nhất một vùng nhớ cố định ngay khi Lớp được nạp vào bộ nhớ.

Ngược lại, một thể hiện (Object Instance) được tạo ra bởi toán tử new sẽ sở hữu vùng nhớ riêng biệt trên Heap để quản lý trạng thái dữ liệu cá nhân (non-static).

Triết lý thiết kế ngôn ngữ và tính minh bạch (Clarity):

Tránh gây nhầm lẫn tư duy: Nếu cho phép gọi thành phần static qua biến thể hiện, lập trình viên dễ hiểu nhầm rằng họ đang thao tác với dữ liệu riêng của đối tượng đó, trong khi thực tế dữ liệu static được chia sẻ chung cho toàn bộ ứng dụng.

C# quy định bắt buộc phải truy xuất trực tiếp qua tên Lớp (ví dụ: TenLop.ThanhPhanStatic) nhằm đảm bảo mã nguồn rõ ràng, minh bạch về ngữ nghĩa và dễ bảo trì.
