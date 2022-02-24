# UnityTheater
# Using Unity v 21.2.12
# Android apk in root folder

Hello good Unity people
Welcome to this mess of a project
Heres what you need to know

1. I am pretty backend -challenged. I really wanted to set up a remote db + server with heroku and clearDB and I really really tried,
  but it wouldnt stay published bc of some auth stuff so I had to give up bc was running out of time and ended up starting some locally stored stuff using SQLite instead.
  
2. Not sure how hard you guys go over there, but I was told this was a 3-4 hour project ??
3. Please excuse this mess of a project I had much bigger and brighter plans for this app, but this week ended up being super busy for me and my brain is mush.
4. I had to balance this project + my current job, family + timezones here in Norway (+ this administrator lady from my college who has to send me a document so I can go home and wont respond for a whole week (visa-stuff))
5. Anyways, it isn't fully functional but I'll include some notes of what I would have done next below, if you would like to read it.

THANK YOU, BE STRONG!






What the plan was:

The tables were gonna go something like this (I got a few of them set up with some sample data, making new table if dne, insert, read from etc)

Movies (id, title, duration)
Screening (screening_id, movie_id, startDat_start_time, auditorium)
TheaterComplex (id, name, address, num of auditoriums)
Auditorium (number, roomlayout/seats)
Transaction (id, reservation/tickets,customer) (maybe point of sale info)
Admin(id, username, password)
Seat_Reserved(id, seat_id, reservation_id,screening_id)
Seat(id, row, column, auditorium, status)
Reservation(id, screening is, emplyee/pos_id, status)

The UI: 

Welcome screen:
Date and movie selection screen 
	(Sick carousel for displaying movie info)
	Only show movies available on selected day
	Disable movies that have already passed 
	Maybe disable or grey out sold out movies 
Movie Showtime screen (probably merge this with the above)
Auditorium view:
	Load seat availability info from db for current auditorium and 
	Pull for updates every second or sth, update ui seat colors accordingly
	Display exits, accessibility info etc etc
	Color legend for seats (available, occupied, out of order, corona, wheel-chair accessible)
Checkout View:
	Register user, create reservation, load into db
	
Admin: lookup Reservation by order number:
  Cancel seats
  
Other:
- 3 types of point of sale (online, kiosk, counter)
- Let admin cancel tickets/release seats

- Let user select party size (limit to 4 ish maybe), if possible suggest consecutive seats in same row if more than oneoption choose closest to center/random/closest to center but with optimizing (not leaving just one seat on either side) next try to split group evenish but ultimately the user would select them independently

- Panasonic-mode:
    Leave at least one seat between groups to ensure social distancing

- Separate and clean up scripts, scenes, assets/folders!
- 
(the orange button only changes seat color to occupied rn nothing else)

https://user-images.githubusercontent.com/25261596/155613343-e5f43d49-ebe5-42c8-b275-fe7620f51fa1.mp4

