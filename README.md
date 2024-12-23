# Ecommerce Application

This repository contains the frontend and backend implementations for an ecommerce application.

- **Frontend**: Built using Xamarin Forms for cross-platform support.
- **Backend**: [Ecommerce Spring Boot Backend](https://github.com/lmontassar/ecommerce-springboot)

## Features

### Admin Features
- **Admin Login**: Secure login with token-based authentication.
- **Dashboard**: Overview of products and categories.
- **Product Management**: Add, edit, and view products.
- **Category Management**: Add, edit, and view categories.

### User Features
- **Cart Management**: Add and remove items from the cart.
- **Product Details**: View detailed information about products.
- **User Registration**: Create a new user account.

## Screenshots

### Login Page
![login](https://github.com/user-attachments/assets/6af669f2-16bb-441b-ba43-18d815d6ca58)

### SignUp Page
![signup](https://github.com/user-attachments/assets/2e003b74-e752-4adc-ba96-d4de94bd530c)

### User List Products
![products](https://github.com/user-attachments/assets/f9426af7-9305-47ed-80be-46b5345af33c)

### User Product Details
![product](https://github.com/user-attachments/assets/0fbb1df1-9c58-44c5-b370-ad5472b3beae)

### User Cart
![panier](https://github.com/user-attachments/assets/415428d2-9637-4044-af64-992086bd2b28)

### Admin Products Management
![admin_prod](https://github.com/user-attachments/assets/54c2f096-4507-4fed-a32f-1d13cf7f05d1)

### Admin Edit/Delete Product
![admin_editprod](https://github.com/user-attachments/assets/1149564e-2bff-4a19-9255-adc7233a532d)

### Admin Add Product
![admin_addprod](https://github.com/user-attachments/assets/1c1f73ec-713c-404f-a01b-10614a2bab48)

### Admin category Management
![admin_categories](https://github.com/user-attachments/assets/8c198815-be60-49a9-b24a-d83c5bea936e)

### Admin Edit/Delete category
![admin_editCat](https://github.com/user-attachments/assets/032d4b34-6d9d-4bc6-b0ff-cc3db0e0bae5)

### Admin Add category
![add_cat](https://github.com/user-attachments/assets/9a267fe7-234c-4bef-8d0e-bf6837dc3cc0)


> **Note**: Replace `path_to_your_screenshot` with the actual folder where the screenshots are stored in the repository.

## How to Run

### Prerequisites
- Visual Studio with Xamarin installed.
- Android/iOS Emulator or a physical device.
- Backend server ([Spring Boot Application](https://github.com/lmontassar/ecommerce-springboot)) running on `http://192.168.0.129(IPV4 for testing):8089/api`.

### Steps
1. Clone this repository:
   ```bash
   git clone https://github.com/lmontassar/ecommerce-xamarin-app.git
   ```
2. Open the solution file (`.sln`) in Visual Studio.
3. Restore NuGet packages.
4. Update the `BaseUrl` in `ApiService.cs` if your backend server has a different IP or port.
5. Build and run the application on an emulator or device.

## Technologies Used

### Frontend
- **Framework**: Xamarin.Forms
- **Styling**: XAML with reusable styles and resources.

### Backend
- **Framework**: Spring Boot
- **Repository**: [Ecommerce Spring Boot Backend](https://github.com/lmontassar/ecommerce-springboot)

### Libraries
- **Newtonsoft.Json**: For JSON serialization and deserialization.
- **HttpClient**: For API requests.

## Folder Structure

### Views
- `AdminDashboardPage.xaml`
- `AdminLoginPage.xaml`
- `CartPage.xaml`
- `CategoryEditPage.xaml`
- `LoginPage.xaml`
- `ProductDetailPage.xaml`
- `ProductEditPage.xaml`
- `ProductListPage.xaml`
- `SignUpPage.xaml`

### Services
- `ApiService.cs`: Handles API requests.

## Contribution

Feel free to submit issues and pull requests to improve the application. 

## License

This project is licensed under the [MIT License](LICENSE).

---

### Contact
- **Developer**: LOUNISSI Montassar
- **Email**: lounissi.montassar@hotmail.com
