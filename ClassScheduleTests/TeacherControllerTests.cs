using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ClassSchedule.Models;
using ClassSchedule.Controllers;
using Moq;
using System.Collections.Generic;

namespace ClassScheduleTests
{
    public class TeacherControllerTests
    {
        [Fact]
        public void IndexActionMethod_ReturnsAViewResult()
        {
            var unit = new Mock<IClassScheduleUnitOfWork>();
            var teachers = new Mock<IRepository<Teacher>>();
            unit.Setup(r => r.Teachers).Returns(teachers.Object);

            var options = new QueryOptions<Teacher>
            {
                OrderBy = t => t.LastName
            };

            teachers.Setup(x => x.List(options))
                .Returns(new List<Teacher>());

            TeacherController controller = new TeacherController(unit.Object);
            var result = controller.Index();

            Assert.Equal(typeof(ViewResult), result.GetType());
        }
    }
}
