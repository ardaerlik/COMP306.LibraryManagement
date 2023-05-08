function CreateScheduler(_id, _startDate, _url) {
    const dp = new DayPilot.Scheduler(_id, {
        startDate: _startDate,
        days: 365,
        scale: "Hour",
        timeHeaders: [
            { groupBy: "Month", format: "MMMM yyyy" },
            { groupBy: "Day", format: "d" },
            { groupBy: "Hour", format: "h"}
        ],
        treeEnabled: true,
        treePreventParentUsage: true,
        heightSpec: "Max",
        height: 400,
        eventMovingStartEndEnabled: true,
        eventResizingStartEndEnabled: true,
        timeRangeSelectingStartEndEnabled: true,
        contextMenu: new DayPilot.Menu({
            items: [
                {
                    text: "Edit",
                    onClick: (args) => {
                        dp.events.edit(args.source);
                    }
                },
                {
                    text: "Delete",
                    onClick: (args) => {
                        dp.events.remove(args.source);
                    }
                },
                { text: "-" },
                {
                    text: "Select",
                    onClick: (args) => {
                        dp.multiselect.add(args.source);
                    }
                }
            ]
        }),
        onEventMoved: (args) => {
            dp.message("Moved: " + args.e.text());
        },
        onEventMoving: (args) => {
            if (args.e.resource() === "A" && args.resource === "B") {
                args.left.enabled = false;
                args.right.html = "You can't move an event from Room 1 to Room 2";

                args.allowed = false;
            }
            else if (args.resource === "B") {
                while (args.start.getDayOfWeek() === 0 || args.start.getDayOfWeek() === 6) {
                    args.start = args.start.addDays(1);
                }
                args.end = args.start.addDays(1);
                args.left.enabled = false;
                args.right.html = "Events in Room 2 must start on a workday and are limited to 1 day.";
            }

            if (args.resource === "C") {
                const except = args.e.data;
                const events = dp.rows.find(args.resource).events.all();

                let start = args.start;
                let end = args.end;
                let overlaps = events.some(item => item.data !== except && DayPilot.Util.overlaps(item.start(), item.end(), start, end));

                while (overlaps) {
                    start = start.addDays(1);
                    end = end.addDays(1);
                    overlaps = events.some(item => item.data !== except && DayPilot.Util.overlaps(item.start(), item.end(), start, end));
                }

                if (args.start !== start) {
                    args.start = start;
                    args.end = end;

                    args.left.enabled = false;
                    args.right.html = "Start automatically moved to " + args.start.toString("d MMMM, yyyy");
                }

            }
        },
        onEventResized: (args) => {
            dp.message("Resized: " + args.e.text());
        },
        onTimeRangeSelected: async (args) => {
            const modal = await DayPilot.Modal.prompt("New event name:", "New Event")
            dp.clearSelection();
            if (modal.canceled) {
                return;
            }
            const name = modal.result;
            dp.events.add({
                start: args.start,
                end: args.end,
                id: DayPilot.guid(),
                resource: args.resource,
                text: name
            });
            dp.message("Created");
        },
        onEventMove: (args) => {
            if (args.ctrl) {
                dp.events.add({
                    start: args.newStart,
                    end: args.newEnd,
                    text: "Copy of " + args.e.text(),
                    resource: args.newResource,
                    id: DayPilot.guid()
                });

                args.preventDefault();
            }
        },
        onEventClick: (args) => {
            DayPilot.Modal.alert(args.e.data.text);
        }
    });

    dp.init();
    dp.scrollTo("2023-03-01");

    const app = {
        barColor(i) {
            const colors = ["#3c78d8", "#6aa84f", "#f1c232", "#cc0000"];
            return colors[i % 4];
        },
        barBackColor(i) {
            const colors = ["#a4c2f4", "#b6d7a8", "#ffe599", "#ea9999"];
            return colors[i % 4];
        },
        loadData() {
            // TODO: Ajax call
            const resources = [
                {
                    name: "Rooms", id: "G1", expanded: true, children: [
                        { name: "Room 1", id: "A" },
                        { name: "Room 2", id: "B" },
                        { name: "Room 3", id: "C" },
                        { name: "Room 4", id: "D" },
                    ]
                },
                {
                    name: "Tables", id: "G2", expanded: true, children: [
                        { name: "Table 1", id: "E" },
                        { name: "Table 2", id: "F" },
                        { name: "Table 3", id: "G" },
                        { name: "Table 4", id: "H" }
                    ]
                }
            ];

            const events = [];
            for (let i = 0; i < 12; i++) {
                const duration = Math.floor(Math.random() * 6) + 1;
                const durationDays = Math.floor(Math.random() * 6) + 1;
                const start = Math.floor(Math.random() * 6) + 2;

                const e = {
                    start: new DayPilot.Date(_startDate).addHours(start),
                    end: new DayPilot.Date(_startDate).addHours(start).addHours(durationDays),
                    id: DayPilot.guid(),
                    resource: String.fromCharCode(65 + i),
                    text: "Event " + (i + 1),
                    bubbleHtml: "Event " + (i + 1),
                    barColor: app.barColor(i),
                    barBackColor: app.barBackColor(i)
                };

                events.push(e);
            }

            dp.update({ resources, events });
        },
    };

    app.loadData();
}
