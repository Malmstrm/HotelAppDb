# HotelAppDb

Eftersom blivit ändring så jag har tagit bort dem "satta" bokningara ifrån starten så kommer inte Syntaxen MSSQL att visa något om man inte skapat bokning själv i appen.

# Syntax
SELECT * 
FROM Customer;

SELECT RoomId, RoomNumber, Type, Price 
FROM Room
WHERE Type = 'Double';

SELECT BookingId, CustomerId, RoomId, CheckInDate, CheckOutDate
FROM Booking
WHERE CheckInDate >= '2025-01-01' AND CheckOutDate <= '2025-12-31';

SELECT CustomerId, FirstName, LastName, Email 
FROM Customer
WHERE Email LIKE '%@example.com';

SELECT RoomId, RoomNumber, Type, Price 
FROM Room
ORDER BY Price ASC;

SELECT BookingId, CustomerId, CheckInDate, CheckOutDate
FROM Booking
WHERE RoomId = 101;

SELECT TOP 5 RoomId, RoomNumber, Type, Price 
FROM Room
ORDER BY Price DESC;

SELECT CustomerId, FirstName, LastName, PhoneNumber 
FROM Customer
WHERE IsActive = 1;

SELECT Room.RoomId, Room.RoomNumber, Room.Type, Room.Price 
FROM Room
LEFT JOIN Booking ON Room.RoomId = Booking.RoomId
WHERE Booking.CheckInDate NOT BETWEEN '2025-01-01' AND '2025-01-30'
  AND Booking.CheckOutDate NOT BETWEEN '2025-01-01' AND '2025-01-30'
  OR Booking.RoomId IS NULL;

SELECT BookingId, CustomerId, RoomId, DATEDIFF(DAY, CheckInDate, CheckOutDate) AS NumberOfNights
FROM Booking
ORDER BY NumberOfNights DESC;