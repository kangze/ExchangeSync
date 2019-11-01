import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';
import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';
import axios from "axios";
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import { ActivityItem, IActivityItemProps, Link, mergeStyleSets, PersonaSize } from 'office-ui-fabric-react';

import { Link as ALink } from 'react-router-dom';

import { Calendar, DateRangeType } from 'office-ui-fabric-react/lib/Calendar';
import Empty from "../_shared/Empty";
import { DatePicker, DayOfWeek, IDatePickerStrings } from 'office-ui-fabric-react';



const classNames = mergeStyleSets({
    exampleRoot: {
        marginTop: '20px',
        marginLeft: 13,
    },
    nameText: {
        fontWeight: 'bold',
        fontSize: 15
    }
});





const DayPickerStrings: IDatePickerStrings = {
    months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    shortMonths: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    days: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],

    shortDays: ['日', '一', '二', '三', '四', '五', '六'],

    goToToday: '转到今天',
    prevMonthAriaLabel: 'Go to previous month',
    nextMonthAriaLabel: 'Go to next month',
    prevYearAriaLabel: 'Go to previous year',
    nextYearAriaLabel: 'Go to next year',
    closeButtonAriaLabel: 'Close date picker'
};

export default class CalendarItem extends React.Component<any, any>{


    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            let user = props.staticContext.user;
            this.state = {
                groups: data.myAppointments,
                loading: false,
                firstDayOfWeek: DayOfWeek.Monday,
                user: user,
                date: new Date(),
                forbiddenDate: data.forbiddenDays.map((u: string) => new Date(u))
            }
        } else if ((window as any).data) {
            let data1 = (window as any).data;
            let user = (window as any).user;
            delete (window as any).data;
            this.state = {
                groups: data1.myAppointments,
                loading: false,
                firstDayOfWeek: DayOfWeek.Monday,
                user: user,
                date: new Date(),
                forbiddenDate: data1.forbiddenDays.map((u: string) => new Date(u))
            }
        } else {
            let user = (window as any).user;
            this.state = { date: new Date(), user: user, loading: false, firstDayOfWeek: DayOfWeek.Monday, groups: [], forbiddenDate: [] };
        }
    }

    componentDidMount() {
        if (this.state.loading)
            return;
        var day2 = new Date();
        this._onSelectDate(day2, null);
    }

    public ToDetail(data: any) {
        let self = this;
        axios.get("/mail/GetMailByAppointmentId?appointmentId=" + data.id).then((response) => {
            var data = response.data;
            self.props.history.push("/detail/" + data.id);
        })
    }

    public createItem(data: any) {
        var date = new Date(data.start);
        var dateStr = date.toLocaleString('chinese', { hour12: false });
        var item = {
            key: 1,
            activityDescription: [
                <Link
                    key={1}
                    className={classNames.nameText}
                >
                    {this.state.user.name}
                </Link>,
                <span key={2}> 创建了 </span>,
                <Link
                    key={1}
                    className={classNames.nameText}
                    onClick={() => {
                        // var func = this.ToDetail.bind(this, data);
                        // func();
                        this.props.history.push("/detail/" + encodeURIComponent(data.mailId))
                    }}
                >
                    {data.title}
                </Link>
            ],
            activityPersonas: [{ size: PersonaSize.size100, imageInitials: this.state.user.name[0] }],
            comments: data.body,
            timeStamp: "开始时间: " + dateStr,
            onClick: () => alert('1'),
            mailId: encodeURIComponent(data.mailId)
        }
        return item;
    }

    private _handleCreateCalendar(e: any) {
        (this.props as any).history.push("/createCalendar");
    }

    private _handleWeekOfDay(day: number) {
        switch (day) {
            case 0:
                return "日";
            case 1:
                return "一";
            case 2:
                return "二";
            case 3:
                return "三";
            case 4:
                return "四";
            case 5:
                return "五";
            case 6:
                return "六";
            default:
                return "未知";
        }
    }

    private _formatDate(date?: Date): string {
        if (!date)
            return "还未选择时间格式";
        return date.toLocaleDateString();
    }

    public _onSelectDate(date: Date, selectedDateRangeArray?: Date[]) {
        let self = this;
        self.setState({ date: date });
        self.setState({
            loading: true,
        })
        axios.get("/Calendar/GetForbidden?year=" + date.getFullYear() + "&month=" + (date.getMonth() + 1) + "&day=" + (date.getDate())).then(response => {
            var forbidenDates = response.data;
            let dates = forbidenDates.map((u: string) => new Date(u));
            self.setState({ forbiddenDate: dates });
            axios.get("/Calendar/MyAppointMents?year=" + date.getFullYear() + "&month=" + (date.getMonth() + 1) + "&day=" + (date.getDate())).then(response => {
                let data = response.data;
                self.setState({
                    groups: data,
                    loading: false,
                })
            }).catch(u => {
                alert("Exchange服务器错误,请重新尝试!");
            })
        }).catch(u => {
            alert("Exchange服务器错误,请重新尝试!");
        });

    }

    private _reander_month(year: number, month: number, data: any) {
        let imgUrl = "/img/" + month + ".jpg";

        return (
            <div>
                <div style={{ width: "100%", height: 200, backgroundImage: 'url(' + imgUrl + ')', backgroundRepeat: "round" }}>
                    <div style={{ paddingLeft: "20%", paddingTop: 20 }}>
                        <Text variant="xLarge" style={{ color: "black" }}>{year}年{month}月</Text>
                    </div>
                </div>
                {data.map((u: any) => {
                    return (
                        <div>
                            <div style={{ marginTop: 5, float: "left", width: "20%", textAlign: "center", height: 40 }}>
                                <Text variant="medium" style={{ color: "black" }}>周{this._handleWeekOfDay(u.week)}</Text>
                                <br />
                                <Text variant="large" style={{ color: "black", fontWeight: 500 }}>{u.day}</Text>
                            </div>
                            <div style={{ marginTop: 10, marginBottom: 10, float: "left", width: "79%", backgroundColor: "#069661", borderRadius: 5, height: 35 }}>
                                <div style={{ paddingTop: 4, paddingLeft: 13 }}>
                                    <Text variant="large" style={{ color: "white" }}>{!u.title ? "暂无标题" : u.title}</Text>
                                </div>
                            </div>
                        </div>
                    );
                })}
                <div style={{ clear: "both" }}></div>
            </div>
        );
    }

    public handleToDetail(id: string) {
        this.props.history.push("/detail/" + id);
    }

    public render() {
        let self = this;
        const { firstDayOfWeek } = this.state;
        var groups = (this.state.groups as any).map((data: any) => self.createItem(data));

        return (
            <div>
                <Calendar
                    onSelectDate={this._onSelectDate.bind(this)}
                    // onDismiss={this._onDismiss}
                    className={"kz"}
                    isMonthPickerVisible={this.props.isMonthPickerVisible}
                    dateRangeType={this.props.dateRangeType}
                    autoNavigateOnSelection={this.props.autoNavigateOnSelection}
                    showGoToToday={true}
                    value={this.state.selectedDate!}
                    firstDayOfWeek={DayOfWeek.Monday}
                    strings={DayPickerStrings}
                    highlightCurrentMonth={true}
                    highlightSelectedMonth={true}
                    isDayPickerVisible={this.props.isDayPickerVisible}
                    showMonthPickerAsOverlay={true}
                    showWeekNumbers={this.props.showWeekNumbers}
                    minDate={this.props.minDate}
                    maxDate={this.props.maxDate}
                    restrictedDates={this.state.forbiddenDate}
                    showSixWeeksByDefault={this.props.showSixWeeksByDefault}
                    workWeekDays={this.props.workWeekDays}
                />

                <hr style={{ width: "75%", border: "none", height: 1, margin: 0, backgroundColor: "#005bac" }} />

                <div style={{ marginTop: 5, marginLeft: 10 }}>
                    <Text key={"1"} variant={"large"} nowrap block>
                        {this.state.date ? this.state.date.getFullYear() + "-" + (this.state.date.getMonth() + 1) + "-" + this.state.date.getDate() + " 日程安排" : "正在加载..."}
                    </Text>
                </div>

                {this.state.loading ?
                    <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
                    :
                    groups.length == 0 ?
                        <Empty calendar={true} />
                        :
                        groups.map((item: any) => <div onClick={this.handleToDetail.bind(this, item.mailId)}><ActivityItem {...item} className={classNames.exampleRoot} /></div>)
                }



                <div style={{ position: "fixed", borderRadius: 42, backgroundColor: "#005bac", height: 47, width: 51, right: 20, bottom: 20, paddingLeft: 15, paddingTop: 17, boxShadow: Depths.depth64 }}>
                    <IconButton
                        iconProps={{
                            iconName: 'CalendarSettings', styles: {
                                root: {
                                    color: "white",
                                    fontSize: 32,
                                }
                            }
                        }}

                        title="Add"
                        ariaLabel="Add"
                        onClick={this._handleCreateCalendar.bind(this)}
                    />
                </div>
            </div>
        );
    }
}