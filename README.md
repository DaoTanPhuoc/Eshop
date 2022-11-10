ASP_EShop_new

Trước khi code sử dụng: git checkout main
Để cập nhật mới nhất: git pull
Chuyển về nhánh bản thân: git checkout <Tên nhánh>
Nếu chưa có branch trên local: git branch <Tên nhánh>
Để kết hợp main về nhánh bản thân: git merge main
Để lưu lên server: Yêu cầu đang ở trong nhánh của bản thân

Sau khi code xong: git add .
Để lưu lại và ghi chú: git commit -m "<Lời nhắn>"
Lưu lại lên server: git push origin <Tên nhánh> Nếu bị lỗi "The following untracked working tree files would be overwritten by merge:" Sử dụng các câu lệnh
git add *
git stash
git pull


//
git branch -m master main
git fetch origin
git branch -u origin/main main
git remote set-head origin -a
