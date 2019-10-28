import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';
import axios from "axios";
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import { ActivityItem, IActivityItemProps, Link, mergeStyleSets } from 'office-ui-fabric-react';

import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';
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
            let user=props.staticContext.user;
            this.state = {
                groups: data,
                loading: false,
                firstDayOfWeek: DayOfWeek.Monday,
                user:user
            }
        } else if ((window as any).data) {
            let data1 = (window as any).data;
            let user=(window as any).user;
            delete (window as any).data;
            this.state = {
                groups: data1,
                loading: false,
                firstDayOfWeek: DayOfWeek.Monday,
                user:user
            }
        } else {
            let user = (window as any).user;
            this.state = {user:user, loading: true, firstDayOfWeek: DayOfWeek.Monday, groups: [] };
        }
    }

    componentDidMount() {
        if (!this.state.loading)
            return;
        var day2 = new Date();
        this._onSelectDate(day2, null);
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
                <span key={3} className={classNames.nameText}>
                    {data.title}
                </span>
            ],
            activityPersonas: [{ imageUrl: "TestImages.personaMale" }],
            comments: data.body,
            timeStamp: "开始时间: " + dateStr,
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
        axios.get("/Calendar/GetForbidden?year=" + date.getFullYear() + "&month=" + (date.getMonth() + 1) + "&day=" + (date.getDate())).then(response => {
            var forbidenDates = response.data;
            let dates = forbidenDates.map((u: string) => new Date(u));
            self.setState({ forbiddenDate: dates });
        });
        self.setState({
            loading: true,
        })
        axios.get("/Calendar/MyAppointMents?year=" + date.getFullYear() + "&month=" + (date.getMonth() + 1) + "&day=" + (date.getDate())).then(response => {
            let data = response.data;
            self.setState({
                groups: data,
                loading: false,
            })
        })
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
                    firstDayOfWeek={this.props.firstDayOfWeek ? this.props.firstDayOfWeek : DayOfWeek.Sunday}
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
                {this.state.loading ?
                    <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
                    :
                    groups.length == 0 ?
                        <Empty calendar={true} />
                        :
                        groups.map((item: any) => <ActivityItem {...item} className={classNames.exampleRoot} />)
                }



                <div style={{ position: "fixed", borderRadius: 42, backgroundColor: "#005bac", height: 47, width: 56, right: 20, bottom: 20, paddingLeft: 8, paddingTop: 17, boxShadow: Depths.depth64 }}>
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