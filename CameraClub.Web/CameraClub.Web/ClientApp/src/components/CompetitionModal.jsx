import React, { Component } from 'react';
import { Container, Row, Col, Label, Input } from 'reactstrap';

export class CompetitionModal extends Component {
    constructor(props) {
        super(props);

        this.state = { id: 0, name: "", hasDigital: false, hasPrint: false };

        this.handleChange = this.handleChange.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleSave() {
        this.props.handleSave(this.state);
    }

    handleClose() {
        this.props.handleClose();
    }

    componentDidUpdate(prevProps) {
        if (!prevProps.show && this.props.show) {
            var data = this.props.competitionData;
            var date = data.date;

            if (!isNaN(Date.parse(date))) {
                date = new Intl.DateTimeFormat('en-us').format(new Date(data.date));
            }

            this.setState(
                {
                    id: data.id,
                    name: data.name,
                    hasDigital: data.hasDigital,
                    hasPrint: data.hasPrint,
                    date: date
                });
        }
    }

    render() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none"}>
                <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{this.state.id === null ? "Add Competition" : "Edit Competition"}</h5>
                            <button type="button" className="close" aria-label="Close" onClick={this.props.handleClose}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <Container className="modal-body">
                            <Row>
                                <Label for="name" sm={2}>Name</Label>
                                <Col sm={10}>
                                    <Input type="text" name="name" placeholder="Name of the competition" value={this.state.name} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="date" sm={2}>Date</Label>
                                <Col sm={10}>
                                    <Input type="text" name="name" placeholder="MM/DD/YYYY" value={this.state.date} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <div className="form-check top-margin-spacing">
                                <input className="form-check-input" type="checkbox" id="hasDigital" checked={this.state.hasDigital} onChange={this.handleChange} />
                                <label className="form-check-label" htmlFor="hasDigital">Digital</label>
                            </div>
                            <div className="form-check">
                                <input className="form-check-input" type="checkbox" id="hasPrint" checked={this.state.hasPrint} onChange={this.handleChange} />
                                <label className="form-check-label" htmlFor="hasPrint">Print</label>
                            </div>
                        </Container>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" onClick={this.handleClose}>Close</button>
                            <button className="btn btn-primary" onClick={this.handleSave}>Save Changes</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}